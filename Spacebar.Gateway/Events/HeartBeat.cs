using Spacebar.Gateway.Models;
using Spacebar.Static.Classes;
using Spacebar.Static.Enums;

namespace Spacebar.Gateway.Events;

public class HeartBeat : IGatewayMessage
{
    public GatewayOpCodes OpCode { get; } = GatewayOpCodes.Heartbeat;

    public async Task Invoke(GatewayPayload payload, WebSocketInfo client)
    {
        client.Lastheartbeat = DateTime.UtcNow;
        Console.WriteLine("Heartbeat");
        await client.SendAsync(new GatewayPayload
        {
            op = GatewayOpCodes.HeartbeatAck
        });
    }
}