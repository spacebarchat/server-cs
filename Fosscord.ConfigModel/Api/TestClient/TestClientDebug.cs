namespace Fosscord.ConfigModel.Api.TestClient;

public class TestClientDebug
{
    public TestClientPatchOptions PatchOptions = new();
    public bool DumpWebsocketTraffic = false;
    public bool DumpWebsocketTrafficToBrowserConsole = false;
}