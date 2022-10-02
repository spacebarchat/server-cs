using System.Net.WebSockets;
using Fosscord.API.Classes;
using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using Fosscord.Gateway.Controllers;
using Fosscord.Gateway.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Constants = Fosscord.Gateway.Models.Constants;

namespace Fosscord.Gateway.Events;

public class Identify : IGatewayMessage
{
    private readonly JWTAuthenticationManager _auth;

    public Identify()
    {
        _auth = new JWTAuthenticationManager();
    }

    public Constants.OpCodes OpCode { get; } = Constants.OpCodes.Identify;

    public async Task Invoke(Payload payload, Websocket client)
    {
        if (payload.d is JObject jObject)
        {
            var identify = jObject.ToObject<Models.Identify>();
            User user = null;
            try
            {
                user = _auth.GetUserFromToken(identify.token);
            }
            catch (Exception e)
            {
                if (GatewayController.Clients.ContainsKey(client))
                    await GatewayController.Clients[client].CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) Constants.CloseCodes.Authentication_failed).ToString(), client.CancellationToken);
                return;
            }

            if (user == null)
            {
                if (GatewayController.Clients.ContainsKey(client))
                    await GatewayController.Clients[client].CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) Constants.CloseCodes.Authentication_failed).ToString(), client.CancellationToken);
                return;
            }

            Db db = Db.GetNewDb();
            client.session_id = RandomStringGenerator.Generate(32);

            var privateUser = new ReadyEvent.PrivateUser()
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
            };
            List<ReadyEvent.PublicRelationShip> relationShips = new List<ReadyEvent.PublicRelationShip>();
            foreach (var rel in db.Relationships.Include(s => s.To).Where(s => s.Id == user.Id))
            {
                ReadyEvent.PublicUser user1 = new ReadyEvent.PublicUser()
                {
                    accent_color = rel.To.AccentColor,
                    avatar = rel.To.Avatar,
                    banner = rel.To.Banner,
                    bio = rel.To.Bio,
                    bot = rel.To.Bot,
                    discriminator = rel.To.Discriminator,
                    id = rel.To.Id,
                    premium_since = new DateTime(),
                    public_flags = rel.To.PublicFlags,
                    username = rel.To.Username
                };

                relationShips.Add(new ReadyEvent.PublicRelationShip()
                {
                    id = rel.Id,
                    nickname = rel.Nickname,
                    type = rel.Type,
                    user = user1
                });
            }

            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(user.Settings);
            var readyEventData = new ReadyEvent.ReadyEventData()
            {
                v = 9,
                application = db.Applications.FirstOrDefault(s => s.Id == user.Id),
                user = privateUser,
                user_settings = user.Settings,
                guilds = db.Members.Where(s => s.Id == user.Id).Select(s => s.Guild).ToList(),
                relationships = relationShips,
                read_state = new ReadyEvent.ReadState()
                {
                    entries = db.ReadStates.Where(s => s.User.Id == user.Id).ToList(),
                    partial = false,
                    version = 304128,
                },
                user_guild_settings = new ReadyEvent.GuildMemberSettings()
                {
                    entries = db.Members.Where(s => s.Id == user.Id).Select(s => s.Settings).ToList(),
                    partial = false,
                    version = 642,
                },
                private_channels = db.Channels
                    .Where(s => (s.Type == 1 || s.Type == 3) && s.Recipients.Any(s => s.Id == user.Id)).ToList(),
                session_id = client.session_id,
                analytics_token = "",
                connected_accounts = db.ConnectedAccounts.Where(s => s.User.Id == user.Id).ToList(),
                consents = new ReadyEvent.Consents()
                {
                    personalization = new ReadyEvent.PersonalizationConsents()
                    {
                        consented = false,
                    }
                },
                country_code = settings.ContainsKey("locale") ? settings["locale"] : "en-us",
                friend_suggestions = 0,
                experiments = new List<object>(),
                guild_join_requests = new List<object>(),
                users = new List<ReadyEvent.PublicUser>(),
                merged_members = db.Members.Where(s => s.Id == user.Id).ToList()
            };

            await GatewayController.Send(client, new Payload()
            {
                d = readyEventData,
                op = Constants.OpCodes.Dispatch,
                t = "READY",
                s = client.sequence++
            });
            client.is_ready = true;

            Console.WriteLine($"Got user {user.Id} {user.Email}");
        }
    }
}