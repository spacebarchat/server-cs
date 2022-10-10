using Fosscord.API;
using Fosscord.API.Middlewares;
using Fosscord.API.Rewrites;
using Fosscord.API.Tasks;
using Fosscord.Util;
using Microsoft.AspNetCore.Rewrite;

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
AspUtils.ConfigureBuilder(ref builder);

var app = builder.Build();

//
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
var defaultRights = Static.Config.Security.Register.DefaultRights;
Console.WriteLine("[DEBUG] Calling setter on config default rights");
Static.Config.Security.Register.DefaultRights = defaultRights;

Static.Config.Save(Environment.GetEnvironmentVariable("CONFIG_PATH") ?? "");
Console.WriteLine("Starting web server!");
app.Run();