using Spacebar.Gateway.Models;
using Spacebar.Static.Classes;
using Spacebar.Static.Enums;

namespace Spacebar.Gateway.Events;

public interface IGatewayMessage
{
    GatewayOpCodes OpCode { get; }
    Task Invoke(GatewayPayload payload, WebSocketInfo client);
}