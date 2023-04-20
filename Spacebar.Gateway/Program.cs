using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using Spacebar.DbModel;
using Spacebar.ConfigModel;
using Spacebar.Gateway.Events;
using Spacebar.Gateway.Models;
using Spacebar.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.IdentityModel.Tokens;
using Sentry;
using Spacebar.Gateway;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { o.LoggingFields = HttpLoggingFields.All; });
builder.Services.AddLogging(o =>
{
    if (SystemdHelpers.IsSystemdService())
        o.AddSystemdConsole();
    else o.AddConsole();

    if (Config.Instance.Sentry.Enabled)
        o.AddSentry(p =>
        {
            p.Dsn = Config.Instance.Sentry.Dsn;
            p.TracesSampleRate = 1.0;
            p.AttachStacktrace = true;
            p.MaxQueueItems = int.MaxValue;
            p.StackTraceMode = StackTraceMode.Original;
            p.Environment = Config.Instance.Sentry.Environment;
            p.Release = GenericUtils.GetVersion();
        });
});
if (Config.Instance.Sentry.Enabled)
{
    Console.WriteLine("Sentry enabled!");
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = Config.Instance.Sentry.Dsn;
        o.TracesSampleRate = 1.0;
        o.AttachStacktrace = true;
        o.MaxQueueItems = int.MaxValue;
        o.StackTraceMode = StackTraceMode.Original;
        o.Environment = Config.Instance.Sentry.Environment;
        o.Release = GenericUtils.GetVersion();
    });
}

builder.Services.AddDbContext<Db>(optionsBuilder =>
{
    var cfg = Config.Instance.DbConfig;
    optionsBuilder
        .UseNpgsql(
            $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true;Maximum Pool Size=1000")
        //.LogTo(str => Debug.WriteLine(str), LogLevel.Information).EnableSensitiveDataLogging().EnableDetailedErrors()
        ;
});
builder.Services.AddScoped(typeof(JwtAuthenticationManager));
builder.Services.AddScoped(typeof(GatewayMessageTypeService));
builder.Services.AddScoped(typeof(WebSocketInfo));

var tokenKey = Config.Instance.Security.JwtSecret;
var key = Encoding.UTF8.GetBytes(tokenKey);

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

Config.Instance.Save(Environment.GetEnvironmentVariable("CONFIG_PATH") ?? "");
if (Config.Instance.Gateway.Debug.WipeOnStartup && Directory.Exists("event_dump")) Directory.Delete("event_dump", true);


// foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
//              .Where(x => typeof(IGatewayMessage).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract))
// {
//     var constructor = type.GetConstructor(Type.EmptyTypes);
//     if (constructor == null)
//     {
//         continue;
//     }
//     else
//     {
//         builder.Services.AddScoped(type);
//         // var message = constructor.Invoke(null) as IGatewayMessage;
//         // if (@message == null)
//         //     continue;
//         // WebSocketInfo.GatewayActions.Add(message.OpCode, message);
//         
//         //Console.WriteLine($"Successfully registered handler for {message.OpCode}");
//     }
// }

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseSentryTracing();

app.UseAuthentication();
//app.UseAuthorization();

app.UseWebSockets();

app.Use((context, next) =>
{
    context.Response.Headers["Content-Type"] += "; charset=utf-8";
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    return next.Invoke();
});
app.UseCors("*");

app.MapControllers();
app.UseDeveloperExceptionPage();

Console.WriteLine("Starting web server!");
if (args.Contains("--exit-on-modified"))
{
    Console.WriteLine("[WARN] --exit-on-modified enabled, exiting on source file change!");
    new FileSystemWatcher()
    {
        Path = Environment.CurrentDirectory,
        Filter = "*.cs",
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    }.Changed += async (sender, args) =>
    {
        Console.WriteLine("Source modified. Exiting...");
        foreach (var webSocketInfo in WebSocketInfo.WebSockets)
            await webSocketInfo.CloseAsync(WebSocketCloseStatus.NormalClosure, "Exiting...");
        await app.StopAsync();
        Environment.Exit(0);
    };
}

app.Run();