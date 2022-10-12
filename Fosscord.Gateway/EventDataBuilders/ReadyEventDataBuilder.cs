using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using Fosscord.Gateway.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fosscord.Gateway.EventDataBuilders;

public class ReadyEventDataBuilder
{
    public static ReadyEvent.ReadyEventData Build(Db db, User user)
    {
        var readyEventData = new ReadyEvent.ReadyEventData
        {
            v = 9,
            application = getApplication(db, user),
            user = getPrivateUser(db, user),
            user_settings = getSettings(db, user),
            guilds = getGuilds(db, user),
            relationships = getRelationships(db, user),
            read_state = getReadStates(db, user),
            user_guild_settings = getUserGuildSettings(db, user),
            private_channels = getDms(db, user),
            session_id = RandomStringGenerator.Generate(32),
            analytics_token = "",
            connected_accounts = getConnectedAccounts(db, user),
            consents = new ReadyEvent.Consents
            {
                personalization = new ReadyEvent.PersonalizationConsents
                {
                    consented = false
                }
            },
            country_code = user.Settings.Locale ?? "en-us",
            friend_suggestions = 0,
            experiments = new List<object>(),
            guild_join_requests = new List<object>(),
            users = getUsers(db,user),
            merged_members = getMergeMembers(db, user)
        };

        return readyEventData;
    }

    private static List<ReadyEvent.PublicUser> getUsers(Db db, User user)
    {
        return new List<ReadyEvent.PublicUser>();
    }

    private static List<ConnectedAccount> getConnectedAccounts(Db db, User user)
    {
        return new List<ConnectedAccount>();
        //return db.ConnectedAccounts.Where(s => s.User.Id == user.Id).ToList();
    }

    private static List<Member> getMergeMembers(Db db, User user)
    {
        return new List<Member>();
        //return db.Members.Where(s => s.Id == user.Id).ToList();
    }

    private static UserSetting? getSettings(Db db, User user)
    {
        return user.Settings;
    }

    private static List<Channel> getDms(Db db, User user)
    {
        return db.Channels
            .Where(s => (s.Type == 1 || s.Type == 3) && s.Recipients.Any(s => s.Id == user.Id)).ToList();
    }

    private static List<Guild> getGuilds(Db db, User user)
    {
        var guilds = db.Members.Where(s => s.Id == user.Id).Select(s => s.GuildId).ToList();
        return db.Guilds.Include(x=>x.Channels)
                        .Include(x=>x.Roles)
                        .Where(x=>guilds.Contains(x.Id))
                        .ToList();
    }

    private static ReadyEvent.GuildMemberSettings getUserGuildSettings(Db db, User user)
    {
        return new ReadyEvent.GuildMemberSettings
        {
            //entries = db.Members.Where(s => s.Id == user.Id).Select(s => s.Settings).ToList(),
            entries = new List<UserChannelSettings>(),
            partial = false,
            version = 642
        };
    }

    private static ReadyEvent.ReadState getReadStates(Db db, User user)
    {
        return new ReadyEvent.ReadState
        {
            //entries = db.ReadStates.Where(s => s.User.Id == user.Id).ToList(),
            entries = new List<ReadState>(),
            partial = false,
            version = 304128
        };
    }

    private static List<ReadyEvent.PublicRelationShip> getRelationships(Db db, User user)
    {
        return db.Relationships.Include(s => s.To).Where(s => s.FromId == user.Id).Select(x => new ReadyEvent.PublicRelationShip
        {
            id = x.Id,
            type = x.Type,
            nickname = x.Nickname,
            user = getPublicUser(db, user)
        }).ToList();
    }

    private static ReadyEvent.PublicUser getPublicUser(Db db, User user)
    {
        return new ReadyEvent.PublicUser
        {
            accent_color = user.AccentColor,
            avatar = user.Avatar,
            banner = user.Banner,
            bio = user.Bio,
            bot = user.Bot,
            discriminator = user.Discriminator,
            id = user.Id,
            premium_since = new DateTime(),
            public_flags = user.PublicFlags,
            username = user.Username
        };
    }

    private static Application getApplication(Db db, User user)
    {
        return db.Applications.FirstOrDefault(s => s.Id == user.Id);
    }

    private static ReadyEvent.PrivateUser getPrivateUser(Db db, User user)
    {
        return new ReadyEvent.PrivateUser
        {
            accent_color = user.AccentColor,
            avatar = user.Avatar,
            banner = user.Banner,
            bio = user.Bio,
            bot = user.Bot,
            desktop = user.Desktop,
            discriminator = user.Discriminator,
            email = user.Email,
            flags = user.Flags,
            id = user.Id,
            username = user.Username,
            mobile = user.Mobile,
            phone = user.Phone,
            premium = user.Premium,
            premium_type = user.PremiumType,
            nsfw_allowed = user.NsfwAllowed,
            mfa_enabled = user.MfaEnabled ?? false,
            verified = user.Verified,
            public_flags = user.PublicFlags,
            premium_since = user.PremiumSince ?? DateTime.Now
        };
    }
}

public class UnavailableGuild
{
    public string id;
    public string unavailable = "true";
}