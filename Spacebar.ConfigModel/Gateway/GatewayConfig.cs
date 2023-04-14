namespace Spacebar.ConfigModel.Gateway;

public class GatewayConfig
{
    public GatewayDebugConfig Debug { get; set; } = new();
    public int HeartbeatInterval { get; set; } = 1000 * 30;
    public int HeartbeatIntervalBuffer { get; set; } = 1000 * 5;
    public int ReadyTimeout { get; set; } = 1000 * 10; //old default = 30s
}