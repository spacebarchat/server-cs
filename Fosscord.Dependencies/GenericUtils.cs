using ArcaneLibs;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Fosscord.Dependencies;

public class GenericUtils
{
    public static string GetVersion()
    {
        string ver = Util.GetCommandOutputSync("git", "rev-parse --short HEAD");
        if (Util.GetCommandOutputSync("git", "status --porcelain").Length > 5) ver += "-unstaged";
        return ver;
    }
}