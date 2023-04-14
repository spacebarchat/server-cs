using Spacebar.Static.Enums;

namespace Spacebar.Static.Classes;
public class GatewayPayload
{
    public GatewayOpCodes op { get; set; }
    public int? s { get; set; }
    public string? t { get; set; }
    public object? d { get; set; }
}