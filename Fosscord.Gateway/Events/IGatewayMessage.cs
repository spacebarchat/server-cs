using Fosscord.Gateway.Models;

namespace Fosscord.Gateway.Events;

public interface IGatewayMessage
{
    Constants.OpCodes OpCode { get; }
    Task Invoke(Payload payload, WebSocketInfo client);
}