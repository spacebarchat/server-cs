using ArcaneLibs;

namespace Fosscord.API; 

public class Config : SaveableObject<Config> {
    public string SentryEnvironment { get; }= "";
}