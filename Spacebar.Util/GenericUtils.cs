namespace Spacebar.Util;

public class GenericUtils
{
    public static string GetVersion()
    {
        string ver = ArcaneLibs.Util.GetCommandOutputSync("git", "rev-parse --short HEAD");
        if (ArcaneLibs.Util.GetCommandOutputSync("git", "status --porcelain").Length > 5) ver += "-unstaged";
        return ver;
    }
}