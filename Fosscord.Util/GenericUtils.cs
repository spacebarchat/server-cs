using Fosscord.DbModel;

namespace Fosscord.Dependencies;

public class GenericUtils
{
    public static string GetVersion()
    {
        string ver = ArcaneLibs.Util.GetCommandOutputSync("git", "rev-parse --short HEAD");
        if (ArcaneLibs.Util.GetCommandOutputSync("git", "status --porcelain").Length > 5) ver += "-unstaged";
        return ver;
    }

    public static string GetSentryEnvironment()
    {
        var envname = FosscordConfig.GetString("sentry_environment", Environment.MachineName);
        if (envname == Environment.MachineName)
        {
            Console.WriteLine("Environment name not set! Using hostname, to change this, set in Config.json!");
            envname = Environment.MachineName;
        }

        return envname;
    }
}