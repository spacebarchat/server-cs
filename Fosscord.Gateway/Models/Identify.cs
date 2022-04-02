namespace Fosscord.Gateway.Models;

public class Identify
{
    public string token;
    public IdentifyProperties properties;
    public Intents intents;
    //todo activity
    public bool compress;
    public int large_threshold;
    public long shard;
    public bool guild_subscriptions;
    public int capabilities;
    public IdentifyClientState client_state;
    public int v;
}

public class IdentifyClientState
{
    //todo: implement
}

public class IdentifyProperties
{
    public string os;
    public string os_atch;
    public string browser;
    public string device;
    public string browser_user_agent;
    public string browser_version;
    public string os_version;
    public string referrer;
    public string referring_domain;
    public string referrer_current;
    public string referring_domain_current;
    public string release_channel;
    public int client_build_number;
    public object client_event_source;
    public string client_version;
    public string system_locale;
}