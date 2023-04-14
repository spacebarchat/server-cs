using ArcaneLibs;
using Spacebar.ConfigModel.Api;
using Spacebar.ConfigModel.Api.TestClient;
using Spacebar.ConfigModel.Gateway;
using Spacebar.DbModel;

namespace Spacebar.ConfigModel;

public class Config : SaveableObject<Config>
{
    public new void Save(string filename = "")
    {
        //deduplicate 
        Gateway.Debug.IgnoredEvents = Gateway.Debug.IgnoredEvents.Distinct().ToArray();
        base.Save(Path);
    }
    public static string Path = "../config.json";
    private static Config _instance;
    public static Config Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Read(Path);
                if (Instance.Sentry.Environment == Environment.MachineName)
                {
                    Console.WriteLine("Sentry environment name not set! Using hostname, to change this, set in Config.json!");
                }
            }
            return _instance;
        }
        [Obsolete("Don't overwrite the running config, you will shoot yourself in the foot!")]
        set => _instance = value;
    }

    public DbConfig DbConfig { get; set; } = new();
    public TestClientConfig TestClient { get; set; } = new();
    public SentryConfig Sentry { get; set; } = new();
    public SecurityConfig Security { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
    public ApiConfig Api { get; set; } = new();
    public GatewayConfig Gateway { get; set; } = new();
    public EndpointConfig Endpoints { get; set; }
}