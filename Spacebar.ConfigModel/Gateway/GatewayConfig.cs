namespace Spacebar.ConfigModel.Gateway;

public class GatewayConfig
{
    public GatewayDebugConfig Debug { get; set; } = new();
    public int HeartbeatInterval { get; set; } = 1000 * 30;
    public int HeartbeatIntervalBuffer { get; set; } = 1000 * 5;
    public int AuthCacheSize { get; set; } = 1000;
    public int GuildCacheSize { get; set; } = 1000;
    public int ChannelCacheSize { get; set; } = 1000;
    public int UserCacheSize { get; set; } = 1000;
    public int MemberCacheSize { get; set; } = 1000;
    
    public int ReadyTimeout { get; set; } = 1000 * 10; //old default = 30s
}