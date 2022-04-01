using System.Diagnostics;
using System.Net;
using System.Text;
using ArcaneLibs;
using Fosscord.API;
using Fosscord.API.Rewrites;
using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Fosscord.Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.IdentityModel.Tokens;
using Sentry;

var cfg = DbConfig.Read();
cfg.Save();
Console.WriteLine("[Database] Applying migrations.");
using (var db = new Db(new DbContextOptionsBuilder<Db>()
           .UseNpgsql(
               $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true")
           .EnableSensitiveDataLogging().Options)) //.LogTo(Debug.WriteLine, LogLevel.Information)
{
    db.Database.Migrate();
    db.SaveChanges();
}

var envname = FosscordConfig.GetString("sentry_environment", Environment.MachineName);
if (envname == Environment.MachineName)
{
    Console.WriteLine("Environment name not set! Using hostname, to change this, set in Config.json!");
    envname = Environment.MachineName;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

    if (FosscordConfig.GetBool("sentry_enabled", true))
        o.AddSentry(p =>
        {
            p.Dsn = "https://b2bf2393e4f64336af713ed9b06f0a9a@sentry.thearcanebrony.net/8";
            p.TracesSampleRate = 1.0;
            p.AttachStacktrace = true;
            p.MaxQueueItems = int.MaxValue;
            p.StackTraceMode = StackTraceMode.Original;
            p.Environment = envname;
            p.Release = GenericUtils.GetVersion();
        });
});
if (FosscordConfig.GetBool("sentry_enabled", true))
{
    Console.WriteLine("Sentry enabled!");
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = "https://b2bf2393e4f64336af713ed9b06f0a9a@sentry.thearcanebrony.net/8";
        o.TracesSampleRate = 1.0;
        o.AttachStacktrace = true;
        o.MaxQueueItems = int.MaxValue;
        o.StackTraceMode = StackTraceMode.Original;
        o.Environment = envname;
        o.Release = GenericUtils.GetVersion();
    });
}

builder.Services.AddDbContextPool<Db>(optionsBuilder =>
{
    optionsBuilder
        .UseNpgsql(
            $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port}")
        .LogTo(str => Debug.WriteLine(str), LogLevel.Information).EnableSensitiveDataLogging();
});

var tokenKey = FosscordConfig.GetString("security_jwtSecret",
    Convert.ToBase64String(Encoding.UTF8.GetBytes(RandomStringGenerator.Generate(255))));
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();

app.UseRouting();
app.UseSentryTracing();

app.UseAuthentication();
// app.UseAuthorization();

app.UseRewriter(new RewriteOptions().Add(new ApiVersionRewriteRule()));

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=FrontendController}/{action=Index}/{id?}");
});
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Use((context, next) =>
{
    context.Response.Headers["Content-Type"] += "; charset=utf-8";
    return next.Invoke();
});

app.MapControllers();

Console.WriteLine("Starting web server!");
app.Run();