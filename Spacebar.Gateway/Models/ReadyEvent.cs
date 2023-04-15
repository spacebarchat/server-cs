using Spacebar.DbModel.Entities;
using Newtonsoft.Json;

namespace Spacebar.Gateway.Models;

public class ReadyEvent
{
    public class PublicRelationShip
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Nickname { get; set; }
        public DbModel.Entities.PublicUser User { get; set; }
    }

    public class PublicUser
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Discriminator { get; set; } = null!;
        public string? Avatar { get; set; }
        public int? AccentColor { get; set; }
        public string? Banner { get; set; }
        public bool Bot { get; set; }
        public string Bio { get; set; } = null!;
        public int PublicFlags { get; set; }
        public DateTime PremiumSince { get; set; } = new();
    }

    public class ReadyEventData
    {
        [JsonProperty("v")] public int Version { get; set; }
        public Application Application { get; set; }


        public PrivateUser User { get; set; }
        public UserSetting UserSettings { get; set; }
        public List<Guild> Guilds { get; set; }
        public List<object> GuildExperiments { get; set; } //todo
        public List<object> GeoOrderedRtcRegions { get; set; } //todo
        public List<PublicRelationShip> Relationships { get; set; }
        public ReadState ReadState { get; set; }
        public GuildMemberSettings UserGuildSettings { get; set; }
        public List<Channel> PrivateChannels { get; set; }
        public string SessionId { get; set; }
        public string AnalyticsToken { get; set; }
        public List<ConnectedAccount> ConnectedAccounts { get; set; }
        public Consents Consents { get; set; }
        public string CountryCode { get; set; }
        public int FriendSuggestions { get; set; }
        public List<object> Experiments { get; set; }
        public List<object> GuildJoinRequests { get; set; }
        public List<PublicUser> Users { get; set; }
        public List<Member> MergedMembers { get; set; }
    }

    public class Consents
    {
        public PersonalizationConsents Personalization { get; set; }
    }

    public class PersonalizationConsents
    {
        public bool Consented { get; set; }
    }

    public class ReadState
    {
        public List<DbModel.Entities.ReadState> Entries { get; set; }
        public bool Partial { get; set; }
        public int Version { get; set; }
    }

    public class GuildMemberSettings
    {
        public List<UserChannelSettings> Entries { get; set; }
        public bool Partial { get; set; }
        public int Version { get; set; }
    }
}