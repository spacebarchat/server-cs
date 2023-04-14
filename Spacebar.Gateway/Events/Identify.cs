
using System.Net.WebSockets;
using Spacebar.DbModel;
using Spacebar.DbModel.Entities;
using Spacebar.Gateway.Events;
using Spacebar.Gateway.Models;
using Spacebar.Static.Classes;
using Spacebar.Static.Enums;
using Spacebar.Util;
using Newtonsoft.Json.Linq;

public class Identify : IGatewayMessage
{
    private readonly JwtAuthenticationManager _auth;

    public Identify()
    {
        _auth = new JwtAuthenticationManager();
    }

    public GatewayOpCodes OpCode { get; } = GatewayOpCodes.Identify;

    public async Task Invoke(GatewayPayload payload, WebSocketInfo client)
    {
        if (payload.d is JObject jObject)
        {
            Db db = Db.GetNewDb();
            var identify = jObject.ToObject<Spacebar.Gateway.Models.Identify>();
            User? user;
            try
            {
                user = _auth.GetUserFromToken(identify.Token);
                var settings = db.UserSettings.FirstOrDefault(x => x.Id == user.Id);
                user.Settings = settings;
            }
            catch (Exception e)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) GatewayCloseCodes.AuthenticationFailed).ToString());
                return;
            }

            if (user is null)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) GatewayCloseCodes.AuthenticationFailed).ToString());
                return;
            }

            var readyEvent = ReadyEventDataBuilder.Build(db, user);
            client.SessionId = readyEvent.SessionId;
            
            await client.SendAsync(new()
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