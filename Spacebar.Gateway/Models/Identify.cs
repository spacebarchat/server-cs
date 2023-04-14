using Spacebar.Util.Rewrites;

namespace Spacebar.Gateway.Models;

public class Identify
{
    public string Token;
    public IdentifyProperties Properties;
    public Intents Intents;
    //todo activity
    public bool Compress;
    public int LargeThreshold;
    public long Shard;
    public bool GuildSubscriptions;
    public int Capabilities;
    public IdentifyClientState ClientState;
    public int V;
}

public class IdentifyClientState
{
    //todo: implement
}

public class IdentifyProperties
{
    public string? Os;
    public string? OsAtch;
    public string? Browser;
    public string? Device;
    public string? BrowserUserAgent;
    public string? BrowserVersion;
    public string? OsVersion;
    public string? Referrer;
    public string? ReferringDomain;
    public string? ReferrerCurrent;
    public string? ReferringDomainCurrent;
    public string? ReleaseChannel;
    public int? ClientBuildNumber;
    public object? ClientEventSource;
    public string? ClientVersion;
    public string? SystemLocale;
}