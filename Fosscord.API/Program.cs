using Fosscord.API.Tasks;
using Fosscord.Util;
using Fosscord.Util.Formatters;

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


AspUtils.ConfigureApp(ref app);
app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=FrontendController}/{action=Index}/{id?}"); });

Console.WriteLine("Starting web server!");
app.Run();