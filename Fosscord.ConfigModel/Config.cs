using ArcaneLibs;
using Fosscord.ConfigModel.Api;
using Fosscord.ConfigModel.Api.TestClient;
using Fosscord.ConfigModel.Gateway;
using Fosscord.DbModel;

namespace Fosscord.ConfigModel;

public class Config : SaveableObject<Config>
{
    public static string Path = "../config.json";
    private static Config _instance;
    public static Config Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Read(Path);
            }
            return _instance;
        }
    }
    public DbConfig DbConfig { get; set; } = new();
    public TestClientConfig TestClient { get; set; } = new();
    public SentryConfig Sentry { get; set; } = new();
    public SecurityConfig Security { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
    public ApiConfig Api { get; set; } = new();
    public GatewayConfig Gateway { get; set; } = new();
}