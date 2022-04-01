using System.Diagnostics;
using System.Text;
using Fosscord.API;
using Fosscord.API.Rewrites;
using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sentry;

var webCfg = Config.Read();
webCfg.Save();

var envname = webCfg.SentryEnvironment;
if (envname.Length < 1) {
    Console.WriteLine("Environment name not set! Using hostname, to change this, set in Config.json!");
    envname = Environment.MachineName;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => o.LoggingFields = HttpLoggingFields.All);
builder.Services.AddLogging(o => o.AddSystemdConsole());
// if(Debugger.IsAttached)
    builder.Services.AddLogging(o => o.AddConsole());
builder.WebHost.UseSentry(o => {
    o.Dsn = "https://b2bf2393e4f64336af713ed9b06f0a9a@sentry.thearcanebrony.net/8";
    o.TracesSampleRate = 1.0;
    o.AttachStacktrace = true;
    o.MaxQueueItems = int.MaxValue;
    o.StackTraceMode = StackTraceMode.Original;
    o.Environment = envname;
});
var cfg = DbConfig.Read();
cfg.Save();
using (var db = new Db(new DbContextOptionsBuilder<Db>().UseNpgsql($"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true")
           .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging().Options)) {
    db.Database.Migrate();
    db.SaveChanges();
}
builder.Services.AddDbContextPool<Db>(optionsBuilder => {
    optionsBuilder.UseNpgsql($"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port}").LogTo(str => Debug.WriteLine(str), LogLevel.Information).EnableSensitiveDataLogging();
});

var tokenKey = FosscordConfig.GetString("security_jwtSecret", Convert.ToBase64String(Encoding.UTF8.GetBytes(RandomStringGenerator.Generate(255))));
var key = Encoding.ASCII.GetBytes(tokenKey);

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

app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Use((context, next) => {
    context.Response.Headers["Content-Type"] += "; charset=utf-8";
    return next.Invoke();
});


// app.UseJwtBearerAuthentication(new JwtBearerOptions()
// {
//     Audience = "http://localhost:5001/", 
//     Authority = "http://localhost:5000/", 
//     AutomaticAuthenticate = true
// });



app.MapControllers();

app.Run();