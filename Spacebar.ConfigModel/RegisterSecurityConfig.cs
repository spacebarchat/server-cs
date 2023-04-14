using System.Collections;
using Spacebar.Static.Enums;
using Newtonsoft.Json;

namespace Spacebar.ConfigModel;

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

                if (Config.Instance.Logging.DefaultRightsDebug) Console.WriteLine($"[DEBUG] Setting default right '{key}' to '{value}'");
                rights[(int) field.GetValue(null)] = value;
            }

            return rights;
        }
        set
        {
            var _rightsDef = typeof(Rights);
            _defaultRights = new Dictionary<string, bool>();
            for (var i = 0; i < value.Length; i++)
            {
                var field = _rightsDef.GetFields()[i];
                if (Config.Instance.Logging.DefaultRightsDebug) Console.WriteLine($"[DEBUG] Setting default right '{field.Name}' to '{value[i]}'");
                _defaultRights.Add(field.Name, value[i]);
            }
        }
    }
}