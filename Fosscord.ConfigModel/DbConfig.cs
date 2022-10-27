using Fosscord.Util.Generic;
using Newtonsoft.Json;

namespace Fosscord.DbModel; 

public class DbConfig
{
    public static DbConfig FromEnv(string name)
    {
        Console.WriteLine("Creating new DB config from environment with name `{0}`", name);
        var cfg = new DbConfig
        {
            Host = EnvUtils.GetEnvironmentVariableOrDefault(name + "_HOST", "localhost"),
            Port = short.Parse(EnvUtils.GetEnvironmentVariableOrDefault(name + "_PORT", "5432")),
            Database = EnvUtils.GetEnvironmentVariableOrDefault(name + "_DATABASE", "fosscord"),
            Username = EnvUtils.GetEnvironmentVariableOrDefault(name + "_USERNAME", "fosscord"),
            Password = EnvUtils.GetEnvironmentVariableOrDefault(name + "_PASSWORD", "fosscord")
        };
        Console.WriteLine("DB config: {0}", JsonConvert.SerializeObject(cfg));
        return cfg;
    }

    public string Driver { get; set; } = EnvUtils.GetEnvironmentVariableOrDefault("PG_DRIVER", "postgres");
    public string Host { get; set; } = EnvUtils.GetEnvironmentVariableOrDefault("PG_HOST", "localhost");
    public string Username { get; set; } = EnvUtils.GetEnvironmentVariableOrDefault("PG_USER", "postgres");
    public string Password { get; set; } = EnvUtils.GetEnvironmentVariableOrDefault("PG_PASS", "postgres");
    public string Database { get; set; } = EnvUtils.GetEnvironmentVariableOrDefault("PG_DB_FOSSCORD_CS", "fosscord_cs");
    public short Port { get; set; } = short.Parse(EnvUtils.GetEnvironmentVariableOrDefault("PG_PORT", "5432"));
}