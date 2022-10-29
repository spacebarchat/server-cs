namespace Fosscord.ConfigModel.Gateway;

public class GatewayConfig
{
    public GatewayDebugConfig Debug { get; } = new();
    public int HeartbeatInterval { get; } = 1000 * 30;
    public int HeartbeatIntervalBuffer { get; } = 1000 * 5;
    public int ReadyTimeout { get; set; } = 1000 * 10; //old default = 30s
}