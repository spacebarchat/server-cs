namespace Fosscord.ConfigModel.Gateway;

public class GatewayDebugConfig
{
    public bool WipeOnStartup { get; } = false;
    public bool DumpGatewayEventsToFiles = false;
    public bool OpenDumpsAfterWrite { get; } = false;
    public (string Command, string Args) OpenDumpCommand { get; } = ("code-insiders", "$file");

    public string[] IgnoredEvents { get; set; } =
    {
        "Heartbeat",
        "Heartbeat_ACK",
    };
}