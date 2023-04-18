using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;
using Spacebar.DbModel;
using Spacebar.DbModel.Entities;
using Spacebar.Gateway.Events;
using Spacebar.Gateway.Models;
using Spacebar.Static.Classes;
using Spacebar.Static.Enums;
using Spacebar.Util;
using Newtonsoft.Json.Linq;
using Spacebar.Static.Utilities;

public class LegacyIdentify : IGatewayMessage
{
    private readonly JwtAuthenticationManager _auth;

    public LegacyIdentify()
    {
        _auth = new JwtAuthenticationManager(db);
    }

    public GatewayOpCodes OpCode { get; } = GatewayOpCodes.LegacyIdentify;

    private Db db = Db.GetNewDb();
    public async Task Invoke(GatewayPayload payload, WebSocketInfo client)
    {
        if (payload.d is JObject jObject)
        {
            var identify = jObject.ToObject<Spacebar.Gateway.Models.Identify>();
            User? user;
            try
            {
                user = await _auth.GetUserFromToken(identify.Token);
                var settings = db.UserSettings.FirstOrDefault(x => x.Id == user.Id);
                user.Settings = settings;
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

            var guilds = db.Members.Where(s => s.Id == user.Id).Select(s => s.GuildId).ToList();
            var readyEvent = new ReadyEvent.ReadyEventData
            {
                Version = 9,
                Application = db.Applications.FirstOrDefault(s => s.Id == user.Id),
                User = user.GetPrivateUser(),
                UserSettings = user.Settings,
                SessionId = client.SessionId,
                AnalyticsToken = "",
                ConnectedAccounts = new List<ConnectedAccount>(),
                CountryCode = user.Settings.Locale ?? "en-us",
                FriendSuggestions = 0,
                Experiments = new List<object>(),
                GuildJoinRequests = new List<object>(),
                Users = new List<ReadyEvent.PublicUser>(),
                MergedMembers = new List<Member>(), //return db.Members.Where(s => s.Id == user.Id).ToList();
                Consents = new ReadyEvent.Consents
                {
                    Personalization = new ReadyEvent.PersonalizationConsents
                    {
                        Consented = false
                    }
                },
                ReadState = new ReadyEvent.ReadState
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
            };

            #region Complex Queries

            readyEvent.PrivateChannels = db.Channels
                .Where(s => (s.Type == 1 || s.Type == 3) && s.Recipients.Any(s => s.Id == user.Id)).ToList();

            readyEvent.Relationships = db.Relationships.Include(s => s.To)
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

            readyEvent.Guilds.AddRange(db.Guilds.Include(x => x.Channels)
                .Include(x => x.Roles)
                .Where(x => guilds.Contains(x.Id))
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
            Console.WriteLine($"User {user.Username}#{user.Discriminator} ({user.Id}) connected");
        }
    }
}