using System.Collections;
using System.Text;
using ArcaneLibs;
using Fosscord.API;
using Fosscord.API.Utilities;
using Fosscord.Shared.Enums;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Fosscord.DbModel;

public class Config : SaveableObject<Config>
{
    public TestClientConfig TestClient { get; set; } = new();
    public SentryConfig Sentry { get; set; } = new();
    public SecurityConfig Security { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
}

public class SecurityConfig
{
    public string JwtSecret { get; set; } = RandomStringGenerator.Generate(255);
    public string? IssuerSigningKey { get; set; } = null!;
    public RegisterSecurityConfig Register { get; set; } = new();
    public LoginSecurityConfig Login { get; set; } = new();
}

public class RegisterSecurityConfig
{
    // ReSharper disable once InconsistentNaming - Required for JSON serialization:
    [JsonProperty("DefaultRights")]
    public Dictionary<string, bool> _defaultRights = new();

    [JsonIgnore]
    public BitArray DefaultRights
    {
        get
        {
            var _rightsDef = typeof(Rights);
            BitArray rights = new(_rightsDef.GetFields().Length);
            foreach (var (key, value) in _defaultRights)
            {
                var field = _rightsDef.GetField(key);
                if (field == null)
                {
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[WARNING] Unknown default right '{key}' in config. Dropping!");
                    Console.ForegroundColor = oldColor;
                    continue;
                }

                if (Static.Config.Logging.DefaultRightsDebug) Console.WriteLine($"[DEBUG] Setting default right '{key}' to '{value}'");
                rights[(int) field.GetValue(null)] = value;
            }

            return rights;
        }
        set
        {
            var _rightsDef = typeof(Rights);
            _defaultRights = new();
            for (int i = 0; i < value.Length; i++)
            {
                var field = _rightsDef.GetFields()[i];
                if (Static.Config.Logging.DefaultRightsDebug) Console.WriteLine($"[DEBUG] Setting default right '{field.Name}' to '{value[i]}'");
                _defaultRights.Add(field.Name, value[i]);
            }
        }
    }
}

public class LoginSecurityConfig
{
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

public class LoggingConfig
{
    public bool DefaultRightsDebug = false;
}

//register config