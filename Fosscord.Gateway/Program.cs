using System.Reflection;
using Fosscord.Gateway.Controllers;
using Fosscord.Gateway.Events;
using Fosscord.Util;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
AspUtils.ConfigureBuilder(ref builder);

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