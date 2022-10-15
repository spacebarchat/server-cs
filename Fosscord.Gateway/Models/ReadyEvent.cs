using Fosscord.DbModel.Scaffold;
using Fosscord.Gateway.EventDataBuilders;

namespace Fosscord.Gateway.Models;

public class ReadyEvent
{
    public class PublicRelationShip
    {
        public string id { get; set; }
        public int type { get; set; }
        public string nickname { get; set; }
        public PublicUser user { get; set; }
    }
    
    public class PublicUser
    {
        public string id { get; set; } = null!;
        public string username { get; set; } = null!;
        public string discriminator { get; set; } = null!;
        public string? avatar { get; set; }
        public int? accent_color { get; set; }
        public string? banner { get; set; }
        public bool bot { get; set; }
        public string bio { get; set; } = null!;
        public int public_flags { get; set; }
        public DateTime premium_since { get; set; } = new();
    }
    
    public class PrivateUser
    {
        public string id { get; set; } = null!;
        public string username { get; set; } = null!;
        public string discriminator { get; set; } = null!;
        public string? avatar { get; set; }
        public int? accent_color { get; set; }
        public string? banner { get; set; }
        public string? phone { get; set; }
        public bool desktop { get; set; }
        public bool mobile { get; set; }
        public bool premium { get; set; }
        public int premium_type { get; set; }
        public bool bot { get; set; }
        public string bio { get; set; } = null!;
        public bool nsfw_allowed { get; set; }
        public bool mfa_enabled { get; set; }
        public bool verified { get; set; }
        public string? email { get; set; }
        public string flags { get; set; } = null!;
        public int public_flags { get; set; }
        public DateTime premium_since { get; set; } = new();
    }

    public class ReadyEventData
    {
        public int v { get; set; }
        public Application application { get; set; }
        
        
        public PrivateUser user { get; set; }
        public UserSetting user_settings { get; set; }
        public List<Guild> guilds { get; set; }
        public List<object> guild_experiments { get; set; } //todo
        public List<object> geo_ordered_rtc_regions { get; set; } //todo
        public List<PublicRelationShip> relationships { get; set; }
        public ReadState read_state { get; set; }
        public GuildMemberSettings user_guild_settings { get; set; }
        public List<Channel> private_channels { get; set; }
        public string session_id { get; set; }
        public string analytics_token { get; set; }
        public List<ConnectedAccount> connected_accounts { get; set; }
        public Consents consents { get; set; }
        public string country_code { get; set; }
        public int friend_suggestions { get; set; }
        public List<object> experiments { get; set; }
        public List<object> guild_join_requests { get; set; }
        public List<PublicUser> users { get; set; }
        public List<Member> merged_members { get; set; }
    }

    public class Consents
    {
        public PersonalizationConsents personalization { get; set; }
    }

    public class PersonalizationConsents
    {
        public bool consented { get; set; }
    }

    public class ReadState
    {
        public List<DbModel.Scaffold.ReadState> entries { get; set; }
        public bool partial { get; set; }
        public int version { get; set; }
    }
    
    public class GuildMemberSettings
    {
        public List<UserChannelSettings> entries { get; set; }
        public bool partial { get; set; }
        public int version { get; set; }
    }
}