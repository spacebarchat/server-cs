using System.Net.WebSockets;
using Fosscord.API.Classes;
using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using Fosscord.Gateway.Controllers;
using Fosscord.Gateway.EventDataBuilders;
using Fosscord.Gateway.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Constants = Fosscord.Gateway.Models.Constants;

namespace Fosscord.Gateway.Events;

public class Identify : IGatewayMessage
{
    private readonly JwtAuthenticationManager _auth;

    public Identify()
    {
        _auth = new JwtAuthenticationManager();
    }

    public Constants.OpCodes OpCode { get; } = Constants.OpCodes.Identify;

    public async Task Invoke(Payload payload, WebSocketInfo client)
    {
        if (payload.d is JObject jObject)
        {
            Db db = Db.GetNewDb();
            var identify = jObject.ToObject<Models.Identify>();
            User? user;
            try
            {
                user = _auth.GetUserFromToken(identify.token);
                var settings = db.UserSettings.FirstOrDefault(x => x.Id == user.Id);
                user.Settings = settings;
            }
            catch (Exception e)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) Constants.CloseCodes.Authentication_failed).ToString());
                return;
            }

            if (user is null)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) Constants.CloseCodes.Authentication_failed).ToString());
                return;
            }

            var readyEvent = ReadyEventDataBuilder.Build(db, user);
            client.SessionId = readyEvent.session_id;
            
            await client.SendAsync(new()
            {
                d = readyEvent,
                op = Constants.OpCodes.Dispatch,
                t = "READY",
                s = client.Sequence++
            });
            client.IsReady = true;
            Console.WriteLine($"User {user.Username}#{user.Discriminator} ({user.Id}) connected");
        }
    }
}