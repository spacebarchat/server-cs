using ArcaneLibs;
using Fosscord.API.Utilities;

namespace Fosscord.DbModel;

public class Config : SaveableObject<Config>
{
    public TestClientConfig TestClient { get; set; } = new();
    public SentryConfig Sentry { get; set; } = new();
    public SecurityConfig Security { get; set; } = new();
}

public class SecurityConfig
{
    public string JwtSecret { get; set; } = RandomStringGenerator.Generate(255);
}

public class SentryConfig
{
    public bool Enabled = true;
    public string Environment = System.Environment.MachineName;
    public string Dsn = "https://b2bf2393e4f64336af713ed9b06f0a9a@sentry.thearcanebrony.net/8";
}

public class TestClientConfig
{
    public bool Enabled = true;
    public bool UseLatest = true;
    public bool Debug = false;
}