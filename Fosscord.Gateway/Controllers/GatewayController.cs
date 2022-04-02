using System.IO.Compression;
using System.Net.WebSockets;
using System.Text;
using Fosscord.DbModel;
using Fosscord.Gateway.Events;
using Fosscord.Gateway.Models;
using Fosscord.Util;
using Ionic.Zlib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fosscord.Gateway.Controllers;

public class GatewayController : Controller
{
    private readonly ILogger<GatewayController> _Logger;
    private readonly Db _db;

    public GatewayController(ILogger<GatewayController> logger, Db db)
    {
        _Logger = logger;
        _db = db;
    }

    public static Dictionary<Constants.OpCodes, IGatewayMessage> GatewayActions = new Dictionary<Constants.OpCodes, IGatewayMessage>();

    public const int HeartbeatInverval = 1000 * 30;
    public const int HeartbeatTimout = 1000 * 45;

    public static Dictionary<Websocket, WebSocket> Clients = new Dictionary<Websocket, WebSocket>();

    [HttpGet("/")]
    public async Task GetWS([FromQuery] string encoding, [FromQuery] int v, [FromQuery] string compress = "")
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _Logger.LogInformation($"Client Connected: {HttpContext.Connection.RemoteIpAddress.ToString()}");
            Websocket clientSocket = new Websocket();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            clientSocket.CancellationToken = cancellationTokenSource.Token;
            clientSocket.encoding = encoding;
            if (encoding != "json" /* && encoding != "etf"*/) //todo etf format
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int)Constants.CloseCodes.Decode_error).ToString(), clientSocket.CancellationToken);
                cancellationTokenSource.Cancel();
                return;
            }
            
            if (v != 9)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int)Constants.CloseCodes.Invalid_API_version).ToString(), clientSocket.CancellationToken);
                cancellationTokenSource.Cancel();
                return;
            }

            clientSocket.compress = compress;
            if (!string.IsNullOrEmpty(compress))
            {
                if (compress != "zlib-stream")
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int)Constants.CloseCodes.Decode_error).ToString(), clientSocket.CancellationToken);
                    cancellationTokenSource.Cancel();
                    return;
                }
            }

            clientSocket.events = new Dictionary<string, Action>();
            clientSocket.member_events = new Dictionary<string, Action>();
            clientSocket.permissions = new Dictionary<string, object>();
            clientSocket.sequence = 0;
            //todo: heartbeat timeout

            Clients.Add(clientSocket, webSocket);

            //TODO: what this this do?
            await SendByteArray(clientSocket, new byte[] { 0x78, 0x9c });
            await Send(clientSocket, new Payload()
            {
                op = Constants.OpCodes.Hello,
                d = new
                {
                    heartbeat_interval = 1000 * 30,
                }
            });
            
            
            Task.Run(() => HeartBeat(clientSocket));
            Task.Run(() => ReadyTimout(clientSocket));

            while (webSocket.State == WebSocketState.Open && !clientSocket.CancellationToken.IsCancellationRequested)
            {
                using (var ms = new MemoryStream())
                {
                    var messageBuffer = WebSocket.CreateClientBuffer(8192, 8192);
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await webSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                        ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    var msgString = Encoding.UTF8.GetString(ms.ToArray());
                    _Logger.LogDebug($"{result.MessageType}\n{msgString}");
                    var message = JsonConvert.DeserializeObject<Payload>(msgString);
                    if (message != null && !string.IsNullOrEmpty(msgString))
                    {
                        if (GatewayActions.ContainsKey(message.op))
                        {
                            try
                            {
                                await GatewayActions[message.op].Invoke(message, clientSocket);
                            }
                            catch (Exception e) //safely disconnect
                            {
                                _Logger.LogError($"Failed to execute GatewayAction\n {e}");
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, ((int)Constants.CloseCodes.Unknown_error).ToString(), clientSocket.CancellationToken);
                                cancellationTokenSource.Cancel();
                            }
                        }
                        else
                        {
                            _Logger.LogWarning($"Client called for missing OP code {message.op}");
                            //add disconnect once done
                        }
                    }
                }
            }

            cancellationTokenSource.Cancel();
            Clients.Remove(clientSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public static async Task Send(Websocket client, Payload payload)
    {
        switch (client.encoding)
        {
            case "json":

                string data = JsonConvert.SerializeObject(payload, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                Console.WriteLine(data);
                var bytes = Encoding.UTF8.GetBytes(data);
                if (client.compress == "zlib-stream")
                {
                    await Clients[client].SendAsync(ZLib.Compress(bytes), WebSocketMessageType.Binary, true, client.CancellationToken);
                }
                break;

            case "etf": //todo: implement
                break;
        }
    }

    public static async Task SendByteArray(Websocket client, byte[] b)
    {
        await Clients[client].SendAsync(b, WebSocketMessageType.Binary, true, client.CancellationToken);
    }

    private async void ReadyTimout(Websocket clientSocket)
    {
        await Task.Delay(1000 * 30);
        if (!clientSocket.is_ready && Clients.ContainsKey(clientSocket))
        {
            await Clients[clientSocket].CloseAsync(WebSocketCloseStatus.NormalClosure, ((int)Constants.CloseCodes.Unknown_error).ToString(), clientSocket.CancellationToken);
        }
    }

    private async void HeartBeat(Websocket clientSocket)
    {
        while (Clients.ContainsKey(clientSocket) && Clients[clientSocket].State == WebSocketState.Open && !clientSocket.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(HeartbeatTimout);
            if (clientSocket.Lastheartbeat.AddSeconds(HeartbeatTimout) < DateTime.UtcNow && Clients.ContainsKey(clientSocket))
            {
                await Clients[clientSocket].CloseAsync(WebSocketCloseStatus.NormalClosure, ((int)Constants.CloseCodes.Session_timed_out).ToString(), clientSocket.CancellationToken);
            }
        }
    }
}