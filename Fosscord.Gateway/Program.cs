using System.Reflection;
using Fosscord.DbModel.Scaffold;
using Fosscord.Gateway.Controllers;
using Fosscord.Gateway.Events;
using Fosscord.Util;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
AspUtils.ConfigureBuilder(ref builder);

var app = builder.Build();

AspUtils.ConfigureApp(ref app);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=FrontendController}/{action=Index}/{id?}");
});

foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof(IGatewayMessage).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract))
{
    var constructor = type.GetConstructor(Type.EmptyTypes);
    if (constructor == null)
    {
        continue;
    }
    else
    {
        IGatewayMessage message = constructor.Invoke(null) as IGatewayMessage;
        if (@message == null) 
            continue;
        GatewayController.GatewayActions.Add(message.OpCode, message);
        Console.WriteLine($"Successfully registered handler for {message.OpCode}");
    }
}

Console.WriteLine("Starting web server!");
app.Run();