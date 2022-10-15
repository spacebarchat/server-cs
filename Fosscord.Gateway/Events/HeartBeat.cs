﻿using Fosscord.Gateway.Controllers;
using Fosscord.Gateway.Models;

namespace Fosscord.Gateway.Events;

public class HeartBeat: IGatewayMessage
{
    public Constants.OpCodes OpCode { get; } = Constants.OpCodes.Heartbeat;

    public async Task Invoke(Payload payload, WebSocketInfo client)
    {
        client.Lastheartbeat = DateTime.UtcNow;
        Console.WriteLine("Heartbeat");
        await GatewayController.Send(client, new Payload()
        {
            op = Constants.OpCodes.Heartbeat_ACK,
        });
    }
}