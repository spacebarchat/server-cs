using Fosscord.Gateway.Models;
using Fosscord.Static.Classes;
using Fosscord.Static.Enums;

namespace Fosscord.Gateway.Events;

public class HeartBeat: IGatewayMessage
{
    public GatewayOpCodes OpCode { get; } = GatewayOpCodes.Heartbeat;

    public async Task Invoke(GatewayPayload payload, WebSocketInfo client)
    {
        client.Lastheartbeat = DateTime.UtcNow;
        Console.WriteLine("Heartbeat");
        await client.SendAsync(new()
        {
            op = GatewayOpCodes.HeartbeatAck,
        });
    }
}