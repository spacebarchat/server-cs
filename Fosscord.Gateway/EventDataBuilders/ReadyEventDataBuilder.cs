using Fosscord.DbModel;
using Fosscord.DbModel.Entities;
using Fosscord.Gateway.Models;
using Fosscord.Static.Utilities;

public class ReadyEventDataBuilder
{
    public static ReadyEvent.ReadyEventData Build(Db db, User user)
    {
        var readyEventData = new ReadyEvent.ReadyEventData
        {
            Version = 9,
            Application = GetApplication(db, user),
            User = GetPrivateUser(db, user),
            UserSettings = GetSettings(db, user),
            Guilds = GetGuilds(db, user),
            Relationships = GetRelationships(db, user),
            ReadState = GetReadStates(db, user),
            UserGuildSettings = GetUserGuildSettings(db, user),
            PrivateChannels = GetDms(db, user),
            SessionId = RandomStringGenerator.Generate(32),
            AnalyticsToken = "",
            ConnectedAccounts = GetConnectedAccounts(db, user),
            Consents = new ReadyEvent.Consents
            {
                Personalization = new ReadyEvent.PersonalizationConsents
                {
                    Consented = false
                }
            },
            CountryCode = user.Settings.Locale ?? "en-us",
            FriendSuggestions = 0,
            Experiments = new List<object>(),
            GuildJoinRequests = new List<object>(),
            Users = GetUsers(db,user),
            MergedMembers = GetMergeMembers(db, user)
        };

        return readyEventData;
    }

    private static List<ReadyEvent.PublicUser> GetUsers(Db db, User user)
    {
        return new List<ReadyEvent.PublicUser>();
    }

    private static List<ConnectedAccount> GetConnectedAccounts(Db db, User user)
    {
        return new List<ConnectedAccount>();
        //return db.ConnectedAccounts.Where(s => s.User.Id == user.Id).ToList();
    }

    private static List<Member> GetMergeMembers(Db db, User user)
    {
        return new List<Member>();
        //return db.Members.Where(s => s.Id == user.Id).ToList();
    }

    private static UserSetting? GetSettings(Db db, User user)
    {
        return user.Settings;
    }

    private static List<Channel> GetDms(Db db, User user)
    {
        return db.Channels
            .Where(s => (s.Type == 1 || s.Type == 3) && s.Recipients.Any(s => s.Id == user.Id)).ToList();
    }

    private static List<Guild> GetGuilds(Db db, User user)
    {
        var guilds = db.Members.Where(s => s.Id == user.Id).Select(s => s.GuildId).ToList();
        return db.Guilds.Include(x=>x.Channels)
                        .Include(x=>x.Roles)
                        .Where(x=>guilds.Contains(x.Id))
                        .ToList();
    }

    private static ReadyEvent.GuildMemberSettings GetUserGuildSettings(Db db, User user)
    {
        return new ReadyEvent.GuildMemberSettings
        {
            //entries = db.Members.Where(s => s.Id == user.Id).Select(s => s.Settings).ToList(),
            Entries = new List<UserChannelSettings>(),
            Partial = false,
            Version = 642
        };
    }

    private static ReadyEvent.ReadState GetReadStates(Db db, User user)
    {
        return new ReadyEvent.ReadState
        {
            //entries = db.ReadStates.Where(s => s.User.Id == user.Id).ToList(),
            Entries = new List<ReadState>(),
            Partial = false,
            Version = 304128
        };
    }

    private static List<ReadyEvent.PublicRelationShip> GetRelationships(Db db, User user)
    {
        return db.Relationships.Include(s => s.To).Where(s => s.FromId == user.Id).Select(x => new ReadyEvent.PublicRelationShip
        {
            Id = x.Id,
            Type = x.Type,
            Nickname = x.Nickname,
            User = GetPublicUser(db, user)
        }).ToList();
    }

    private static ReadyEvent.PublicUser GetPublicUser(Db db, User user)
    {
        return new ReadyEvent.PublicUser
        {
            AccentColor = user.AccentColor,
            Avatar = user.Avatar,
            Banner = user.Banner,
            Bio = user.Bio,
            Bot = user.Bot,
            Discriminator = user.Discriminator,
            Id = user.Id,
            PremiumSince = new DateTime(),
            PublicFlags = user.PublicFlags,
            Username = user.Username
        };
    }

    private static Application GetApplication(Db db, User user)
    {
        return db.Applications.FirstOrDefault(s => s.Id == user.Id);
    }

    private static ReadyEvent.PrivateUser GetPrivateUser(Db db, User user)
    {
        return new ReadyEvent.PrivateUser
        {
            AccentColor = user.AccentColor,
            Avatar = user.Avatar,
            Banner = user.Banner,
            Bio = user.Bio,
            Bot = user.Bot,
            Desktop = user.Desktop,
            Discriminator = user.Discriminator,
            Email = user.Email,
            Flags = user.Flags,
            Id = user.Id,
            Username = user.Username,
            Mobile = user.Mobile,
            Phone = user.Phone,
            Premium = user.Premium,
            PremiumType = user.PremiumType,
            NsfwAllowed = user.NsfwAllowed,
            MfaEnabled = user.MfaEnabled ?? false,
            Verified = user.Verified,
            PublicFlags = user.PublicFlags,
            PremiumSince = user.PremiumSince ?? DateTime.Now
        };
    }
}

public class UnavailableGuild
{
    public string id;
    public string unavailable = "true";
}