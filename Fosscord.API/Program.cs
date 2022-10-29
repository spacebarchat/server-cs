using System.Text;
using Fosscord.API.Middlewares;
using Fosscord.API.Tasks;
using Fosscord.DbModel;
using Fosscord.ConfigModel;
using Fosscord.Util;
using Fosscord.Util.Rewrites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.IdentityModel.Tokens;
using Sentry;

if (!Directory.Exists("cache_formatted")) Directory.CreateDirectory("cache_formatted");
if (!Directory.Exists("cache")) Directory.CreateDirectory("cache");
/*foreach (var file in Directory.GetFiles("cache").Where(x => x.EndsWith(".js")))
{
    //JsFormatter.FormatJsFile(File.OpenRead(file), File.OpenWrite(file.Replace("cache", "cache_formatted")));
}*/

/*var processes = Directory.GetFiles("cache").Where(x => x.EndsWith(".js")).Select(file => JsFormatter.SafeFormat(file, file.Replace("cache", "cache_formatted"))).ToList();
while (processes.Any(x => !x.HasExited))
{
    Thread.Sleep(100);
}*/


//Environment.Exit(0);
Tasks.RunStartup();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpLogging(o => { o.LoggingFields = HttpLoggingFields.All; });
        builder.Services.AddLogging(o =>
        {
            if (SystemdHelpers.IsSystemdService())
            {
                o.AddSystemdConsole();
            }
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
                    $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true")
                //.LogTo(str => Debug.WriteLine(str), LogLevel.Information).EnableSensitiveDataLogging().EnableDetailedErrors()
                ;
        });
        builder.Services.AddSingleton(new JwtAuthenticationManager());

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

var app = builder.Build();

//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseRouting();
app.UseSentryTracing();

app.UseAuthentication();
//app.UseAuthorization();


app.UseRewriter(new RewriteOptions().Add(new ApiVersionRewriteRule()));
app.UseWebSockets();

app.UseMiddleware<RightsMiddleware>();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Use((context, next) =>
{
    context.Response.Headers["Content-Type"] += "; charset=utf-8";
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    return next.Invoke();
});
app.UseCors("*");

app.MapControllers();
app.UseDeveloperExceptionPage();

//
app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=FrontendController}/{action=Index}/{id?}"); });


Console.WriteLine("[DEBUG] Calling getter on config default rights");
var defaultRights = Config.Instance.Security.Register.DefaultRights;
Console.WriteLine("[DEBUG] Calling setter on config default rights");
Config.Instance.Security.Register.DefaultRights = defaultRights;

Config.Instance.Save(Environment.GetEnvironmentVariable("CONFIG_PATH") ?? "");
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
        await app.StopAsync();
        Environment.Exit(0);
    };
}
app.Run();