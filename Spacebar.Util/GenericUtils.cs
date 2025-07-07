namespace Spacebar.Util;

public class GenericUtils {
    [Obsolete("FIXME: Use proper versioning!")]
    public static string GetVersion() {
        var ver = ArcaneLibs.Util.GetCommandOutputSync("git", "rev-parse --short HEAD");
        if (ArcaneLibs.Util.GetCommandOutputSync("git", "status --porcelain").Length > 5) ver += "-unstaged";
        return ver;
    }
}