using Fosscord.DbModel;

namespace Fosscord.API;

public static class Static
{
    public static Config Config { get; set; } = Config.Read(Environment.GetEnvironmentVariable("CONFIG_PATH") ?? "");
}