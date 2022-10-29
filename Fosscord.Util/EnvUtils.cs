namespace Fosscord.Util;

public class EnvUtils
{
    public static string GetEnvironmentVariableOrDefault(string name, string defaultValue)
    {
        Console.WriteLine($"Getting environment variable '{name}', or using '{defaultValue}' as default...");
        return Environment.GetEnvironmentVariable(name) ?? defaultValue;
    }
}