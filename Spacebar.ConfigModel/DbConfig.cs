using Spacebar.Util;
using Newtonsoft.Json;

namespace Spacebar.DbModel;

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

    public string Driver { get; set; } = "postgres";
    public string Host { get; set; } = "localhost";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "postgres";
    public string Database { get; set; } = "fosscord_cs";
    public short Port { get; set; } = 5432;
}