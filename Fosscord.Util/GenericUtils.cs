﻿using Fosscord.ConfigModel;

namespace Fosscord.Util;

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
        var envname = Config.Instance.Sentry.Environment;
        if (envname == Environment.MachineName)
        {
            Console.WriteLine("Environment name not set! Using hostname, to change this, set in Config.json!");
        }

        return envname;
    }
}