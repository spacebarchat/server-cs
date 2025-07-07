using System.Reflection;
using Spacebar.DbModel;
using Spacebar.Gateway.Events;
using Spacebar.Static.Enums;
using Spacebar.Util;

namespace Spacebar.Gateway;

public class GatewayMessageTypeService
{
    public readonly Dictionary<GatewayOpCodes, IGatewayMessage> GatewayActions = new();
    public GatewayMessageTypeService(Db _db, JwtAuthenticationManager _auth)
    {
        var services = new ServiceCollection();
        services.AddSingleton(_db);
        services.AddSingleton(_auth);
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                     .Where(x => typeof(IGatewayMessage).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract))
        {
            //type.GetConstructors()
            // var constructor = type.GetConstructor(Type.EmptyTypes);
            // if (constructor == null)
            // {
                // continue;
            // }

            // var message = constructor.Invoke(null) as IGatewayMessage;
            // if (@message == null)
                // continue;
            // GatewayActions.Add(message.OpCode, message);
            services.AddScoped(typeof(IGatewayMessage),type);
            

            //Console.WriteLine($"Successfully registered handler for {message.OpCode}");
        }
        var provider = services.BuildServiceProvider();
        foreach (var service in provider.GetServices(typeof(IGatewayMessage)))
        {
            var svc = service as IGatewayMessage;
            GatewayActions.Add(svc.OpCode, svc);
            Console.WriteLine($"Successfully registered handler for {svc.OpCode}");
        }
    }
}