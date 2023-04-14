namespace Spacebar.ConfigModel.Gateway;

public class GatewayDebugConfig
{
    public bool WipeOnStartup { get; set; } = false;
    public bool DumpGatewayEventsToFiles = false;
    public bool OpenDumpsAfterWrite { get; set; } = false;
    public (string Command, string Args) OpenDumpCommand { get; set; } = ("code-insiders", "$file");

    public string[] IgnoredEvents { get; set; } =
    {
        "Heartbeat",
        "Heartbeat_ACK",
    };
}