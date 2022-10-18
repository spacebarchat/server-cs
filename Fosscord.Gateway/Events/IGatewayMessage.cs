using Fosscord.Gateway.Models;
using Fosscord.Static.Classes;
using Fosscord.Static.Enums;

namespace Fosscord.Gateway.Events;

public interface IGatewayMessage
{
    GatewayOpCodes OpCode { get; }
    Task Invoke(GatewayPayload payload, WebSocketInfo client);
}