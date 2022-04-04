using System.Runtime.CompilerServices;
using ArcaneLibs.Logging;
using ArcaneLibs.Logging.LogEndpoints;

namespace Fosscord.Util.Formatters;

public class Formatter
{
    private static LogManager log = new LogManager() {Prefix = "[Formatter] "};

    internal static LogManager GetLog([CallerFilePath] string name = "Formatter")
    {
        var l = new LogManager()
        {
            Prefix = "[" + name.Split("/").Last() + "] ",
            LogTime = true
        };
        l.AddEndpoint(new ConsoleEndpoint());
        return l;
    }
}