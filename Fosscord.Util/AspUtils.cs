using System.Diagnostics;
using System.Text;
using Fosscord.API;
using Fosscord.API.Classes;
using Fosscord.API.Rewrites;
using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Fosscord.Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sentry;

namespace Fosscord.Util;

public class AspUtils
{
    public static void ConfigureBuilder(ref WebApplicationBuilder builder)
    {
        var cfg = DbConfig.Read();
        cfg.Save();

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

            if (Static.Config.Sentry.Enabled)
                o.AddSentry(p =>
                {
                    p.Dsn = Static.Config.Sentry.Dsn;
                    p.TracesSampleRate = 1.0;
                    p.AttachStacktrace = true;
                    p.MaxQueueItems = int.MaxValue;
                    p.StackTraceMode = StackTraceMode.Original;
                    p.Environment = GenericUtils.GetSentryEnvironment();
                    p.Release = GenericUtils.GetVersion();
                });
        });
        if (Static.Config.Sentry.Enabled)
        {
            Console.WriteLine("Sentry enabled!");
            builder.WebHost.UseSentry(o =>
            {
                o.Dsn = Static.Config.Sentry.Dsn;
                o.TracesSampleRate = 1.0;
                o.AttachStacktrace = true;
                o.MaxQueueItems = int.MaxValue;
                o.StackTraceMode = StackTraceMode.Original;
                o.Environment = GenericUtils.GetSentryEnvironment();
                o.Release = GenericUtils.GetVersion();
            });
        }

        builder.Services.AddDbContextPool<Db>(optionsBuilder =>
        {
            var cfg = DbConfig.Read();
            cfg.Save();
            optionsBuilder
                .UseNpgsql(
                    $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port}")
                .LogTo(str => Debug.WriteLine(str), LogLevel.Information).EnableSensitiveDataLogging();
        });
        builder.Services.AddSingleton(new JwtAuthenticationManager());

        var tokenKey = Static.Config.Security.JwtSecret;
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
    }

    public static void ConfigureApp(ref WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseSentryTracing();

        app.UseAuthentication();
        //app.UseAuthorization();


        app.UseRewriter(new RewriteOptions().Add(new ApiVersionRewriteRule()));
        app.UseWebSockets();

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
    }
}