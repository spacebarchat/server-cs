namespace Fosscord.ConfigModel.Api.TestClient;

public class TestClientConfig
{
    public bool Enabled = true;
    public bool UseLatest = true;
    public TestClientDebug DebugOptions = new();
}