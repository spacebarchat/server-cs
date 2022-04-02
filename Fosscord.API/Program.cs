using Fosscord.API.Tasks;
using Fosscord.Util;

Tasks.RunStartup();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
AspUtils.ConfigureBuilder(ref builder);

var app = builder.Build();

AspUtils.ConfigureApp(ref app);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=FrontendController}/{action=Index}/{id?}");
});

Console.WriteLine("Starting web server!");
app.Run();