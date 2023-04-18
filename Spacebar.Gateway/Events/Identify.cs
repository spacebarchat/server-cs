using System.Collections;
using System.Diagnostics;
using System.Net.WebSockets;
using ArcaneLibs.Extensions;
using Microsoft.EntityFrameworkCore;
using Spacebar.DbModel;
using Spacebar.DbModel.Entities;
using Spacebar.Gateway.Events;
using Spacebar.Gateway.Models;
using Spacebar.Static.Classes;
using Spacebar.Static.Enums;
using Spacebar.Util;
using Newtonsoft.Json.Linq;
using Sentry;
using Spacebar.Gateway.Models.DTO;
using User = Spacebar.DbModel.Entities.User;

public class Identify : IGatewayMessage
{
    private readonly JwtAuthenticationManager _auth;

    public Identify()
    {
        _auth = new JwtAuthenticationManager(db);
    }

    public GatewayOpCodes OpCode { get; } = GatewayOpCodes.Identify;
    private Db db = Db.GetNewDb();

    public async Task Invoke(GatewayPayload payload, WebSocketInfo client)
    {
        var transaction = SentrySdk.StartTransaction("Gateway", "Identify");
        var sw = Stopwatch.StartNew();
        SentrySdk.ConfigureScope(scope => { scope.Transaction = transaction; });
        if (payload.d is JObject jObject)
        {
            var identify = jObject.ToObject<Spacebar.Gateway.Models.Identify>();
            User? user;
            try
            {
                user = await _auth.GetUserFromToken(identify.Token);
            }
            catch (Exception e)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    ((int)GatewayCloseCodes.AuthenticationFailed).ToString());
                return;
            }

            if (user is null)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    ((int)GatewayCloseCodes.AuthenticationFailed).ToString());
                return;
            }

            user.Settings = await db.UserSettings.FirstOrDefaultAsync(x => x.Id == user.Id);
            var guilds = await db.Members.Where(s => s.Id == user.Id).Select(s => s.GuildId).ToListAsync();
            var readyEvent = new ReadyEvent.ReadyEventData
            {
                Version = 9,
                SessionId = client.SessionId,
                UserSettings = user.Settings,
                AnalyticsToken = "",
                ConnectedAccounts = new List<ConnectedAccount>(),
                FriendSuggestions = 0,
                Experiments = new List<object>(),
                GuildJoinRequests = new List<object>(),
                Users = new List<ReadyEvent.PublicUser>(),
                MergedMembers = new List<Member>(),
                CountryCode = user.Settings.Locale ?? "en-us",
                ReadState = new()
                {
                    //entries = db.ReadStates.Where(s => s.User.Id == user.Id).ToList(),
                    Entries = new List<ReadState>(),
                    Partial = false,
                    Version = 304128
                },
                UserGuildSettings = new ReadyEvent.GuildMemberSettings
                {
                    //entries = db.Members.Where(s => s.Id == user.Id).Select(s => s.Settings).ToList(),
                    Entries = new List<UserChannelSettings>(),
                    Partial = false,
                    Version = 642
                }
                //return db.Members.Where(s => s.Id == user.Id).ToList();
            };
            readyEvent.Application = db.Applications.FirstOrDefault(s => s.Id == user.Id);
            readyEvent.User = user.GetPrivateUser();

            #region Complex Queries

            readyEvent.Consents = new ReadyEvent.Consents
            {
                Personalization = new ReadyEvent.PersonalizationConsents
                {
                    Consented = false
                }
            };

            readyEvent.PrivateChannels = db.Channels
                .AsNoTracking()
                .Where(s => (s.Type == 1 || s.Type == 3) && s.Recipients.Any(s => s.Id == user.Id)).ToList();

            readyEvent.Relationships = db.Relationships
                .AsNoTracking()
                .Include(s => s.To)
                .Where(s => s.FromId == user.Id)
                .Select(x =>
                    new ReadyEvent.PublicRelationShip
                    {
                        Id = x.Id,
                        Type = x.Type,
                        Nickname = x.Nickname,
                        User = x.To.GetPublicUser()
                    }
                ).ToList();

            /*readyEvent.Guilds = new ArrayList
            {
                db.Guilds.Include(x => x.Channels)
                    .Include(x => x.Roles)
                    .Where(x => guilds.Contains(x.Id))
                    .ToList()
            };*/

            readyEvent.Guilds = new ArrayList(db.Guilds
                .AsNoTracking()
                .IgnoreAutoIncludes()
                .Include(x => x.Channels)
                .Include(x => x.Roles)
                .Where(x => guilds.Contains(x.Id)).AsEnumerable()
                .Select(ToReadyGuildAsync)
                .ToList());

            #endregion

            await client.SendAsync(new GatewayPayload
            {
                d = readyEvent,
                op = GatewayOpCodes.Dispatch,
                t = "READY",
                s = client.Sequence++
            });
            client.IsReady = true;
            Console.WriteLine(
                $"User {user.Username}#{user.Discriminator} ({user.Id}) connected, sent READY in {sw.ElapsedMilliseconds} ms");
            sw.Stop();

            transaction.Finish();

            // TODO: can we reduce this?
            ReadyGuildDTO ToReadyGuildAsync(Guild guild)
            {
                var span = transaction.StartChild("ToReadyGuildAsync");
                var rg = new ReadyGuildDTO();
                rg.channels = guild.Channels.ToArray();
                rg.emojis = guild.Emojis.ToArray();
                rg.members = guild.Members.ToArray();
                rg.roles = guild.Roles.ToArray();
                rg.stickers = guild.Stickers.ToArray();
                rg.id = guild.Id;
                rg.data_mode = "full";
                rg.lazy = true;
                rg.version = "1";
                rg.threads = Array.Empty<object>();
                rg.stage_instances = Array.Empty<object>();
                rg.guild_scheduled_events = Array.Empty<object>();
                rg.properties = new ReadyGuildProperties();
                rg.properties.name = guild.Name;
                rg.properties.description = guild.Description;
                rg.properties.icon = guild.Icon;
                rg.properties.splash = guild.Splash;
                rg.properties.banner = guild.Banner;
                rg.properties.features = guild.Features.Split(',');
                rg.properties.preferred_locale = guild.PreferredLocale;
                rg.properties.owner_id = guild.OwnerId;
                rg.properties.afk_channel_id = guild.AfkChannelId;
                rg.properties.afk_timeout = guild.AfkTimeout;
                rg.properties.system_channel_id = guild.SystemChannelId;
                rg.properties.verification_level = guild.VerificationLevel;
                rg.properties.explicit_content_filter = guild.ExplicitContentFilter;
                rg.properties.default_message_notifications = guild.DefaultMessageNotifications;
                rg.properties.mfa_level = guild.MfaLevel;
                rg.properties.premium_tier = guild.PremiumTier;
                rg.properties.premium_progress_bar_enable = guild.PremiumProgressBarEnabled ?? false;
                rg.properties.system_channel_flags = guild.SystemChannelFlags;
                rg.properties.discovery_splash = guild.DiscoverySplash;
                rg.properties.rules_channel_id = guild.RulesChannelId;
                rg.properties.public_updates_channel_id = guild.PublicUpdatesChannelId;
                rg.properties.max_video_channel_users = guild.MaxVideoChannelUsers;
                rg.properties.max_members = guild.MaxMembers;
                rg.properties.nsfw_level = guild.NsfwLevel;
                span.Finish();
                return rg;
            }
        }
    }
}