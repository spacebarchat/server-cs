using Fosscord.Static.Enums;

namespace Fosscord.Static.Classes;
public class GatewayPayload
{
    public GatewayOpCodes op { get; set; }
    public int? s { get; set; }
    public string? t { get; set; }
    public object? d { get; set; }
}