using ArcaneLibs;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Hosting.Systemd;
using ReferenceClientProxyImplementation.Configuration;
using ReferenceClientProxyImplementation.Services;
using ReferenceClientProxyImplementation.Tasks;

// using Spacebar.API.Tasks.Startup;

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
// Tasks.RunStartup();

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHostOptions(host => {
    host.ServicesStartConcurrently = true;
    host.ServicesStopConcurrently = true;
    host.ShutdownTimeout = TimeSpan.FromSeconds(5);
});

// builder.Services.AddHostedService<TemporaryTestJob>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { o.LoggingFields = HttpLoggingFields.All; });
builder.Services.AddLogging(o => {
    if (SystemdHelpers.IsSystemdService())
        o.AddSystemdConsole();
    else o.AddConsole();
});

builder.Services.AddSingleton<ProxyConfiguration>();
// builder.Services.AddSingleton<BuildDownloadService>();

foreach (var taskType in ClassCollector<ITask>.ResolveFromAllAccessibleAssemblies())
{
    builder.Services.AddSingleton(typeof(ITask), taskType);
}
builder.Services.AddHostedService<Tasks>();

var app = builder.Build();

//
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.UseRouting();
// app.UseSentryTracing();

// app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Use((context, next) => {
    context.Response.Headers["Content-Type"] += "; charset=utf-8";
    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    return next.Invoke();
});
app.UseCors("*");

app.MapControllers();
app.UseDeveloperExceptionPage();

//
app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=FrontendController}/{action=Index}/{id?}"); });

Console.WriteLine("Starting web server!");
if (args.Contains("--exit-on-modified")) {
    Console.WriteLine("[WARN] --exit-on-modified enabled, exiting on source file change!");
    new FileSystemWatcher {
        Path = Environment.CurrentDirectory,
        Filter = "*.cs",
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    }.Changed += async (sender, args) => {
        Console.WriteLine("Source modified. Exiting...");
        await app.StopAsync();
        Environment.Exit(0);
    };
}

app.Run();