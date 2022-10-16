using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Fosscord.API;
using Fosscord.Gateway.Events;
using Fosscord.Util;
using Ionic.Zlib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fosscord.Gateway.Models;

public class WebSocketInfo : IDisposable
{
    private bool _disposed = false;
    public int Version;
    public string UserId;
    public string SessionId;
    public string Encoding;
    public string? Compress;
    public long? ShardCount;

    public long? ShardId;

    //public deflate?: Deflate; todo:????????
    public DateTime Lastheartbeat = DateTime.MaxValue;
    public bool IsReady = false;
    public Intents Intents;
    public int Sequence = 0;
    public Dictionary<string, object> Permissions = new(); //todo: permissions
    public Dictionary<string, Action> Events = new();
    public Dictionary<string, Action> MemberEvents = new();
    public object ListenOptions;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private CancellationToken _cancellationToken;
    private WebSocket _webSocket;
    private ZlibStream? _zlibStream;
    private MemoryStream? _zlibInputStream;

    public WebSocketInfo(string encoding, int v, string compress = "")
    {
        this.Encoding = encoding;
        Version = v;
        this.Compress = compress;
        _cancellationToken = _cancellationTokenSource.Token;
    }

    public async Task AcceptWebSocketAsync(WebSocketManager manager)
    {
        _webSocket = await manager.AcceptWebSocketAsync();
        var isValid = await ValidateWebsocketParameters();
        if (isValid)
        {
            await SendAsync(HelloPayload);

#pragma warning disable CS4014 Ignore not awaiting task
            Task.Run(HandleTimeouts, _cancellationToken);
#pragma warning restore CS4014
            await RunReceiveLoop();
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to cancel token source for {UserId}: {e.Message}");
            }

            Dispose();
        }
    }

    public async Task RunReceiveLoop()
    {
        while (_webSocket.State == WebSocketState.Open && !_cancellationToken.IsCancellationRequested)
        {
            using var ms = new MemoryStream();
            var messageBuffer = WebSocket.CreateClientBuffer(65535, 65535);
            WebSocketReceiveResult result;
            do
            {
                result = await _webSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            var msgString = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            //Console.WriteLine($"{result.MessageType}\n{msgString}");
            var message = JsonConvert.DeserializeObject<Payload>(msgString);
            if (message != null && !string.IsNullOrEmpty(msgString))
            {
                //dump gateway events
                await DumpPayloadToFile(message, false);

                if (GatewayActions.ContainsKey(message.op))
                {
                    try
                    {
                        await GatewayActions[message.op].Invoke(message, this);
                    }
                    catch (Exception e) //safely disconnect
                    {
                        Console.WriteLine($"Failed to execute GatewayAction\n {e}");
                        await CloseAsync(WebSocketCloseStatus.NormalClosure,
                            ((int) Constants.CloseCodes.Unknown_error).ToString());
                        _cancellationTokenSource.Cancel();
                    }
                }
                else
                {
                    Console.WriteLine($"Client called for missing OP code {message.op}");
                    //write to file
                    if (!Directory.Exists("unknown_events")) Directory.CreateDirectory("unknown_events");
                    if (!Directory.Exists($"unknown_events/{message.op}"))
                        Directory.CreateDirectory($"unknown_events/{message.op}");
                    File.WriteAllText(
                        $"unknown_events/{message.op}/{Directory.GetFiles($"unknown_events/{message.op}").Length}.json",
                        JsonConvert.SerializeObject(JsonConvert.DeserializeObject<dynamic>(msgString), Formatting.Indented));
                    //add disconnect once done
                }
            }
        }
    }

    public async Task CloseAsync(WebSocketCloseStatus status, string reason)
    {
        if (_webSocket.State == WebSocketState.Open)
            try
            {
                await _webSocket.CloseAsync(status, reason, _cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not close websocket: {e.Message}");
            }

        Dispose();
    }

    private async Task<bool> ValidateWebsocketParameters()
    {
        if (Encoding != "json" /* && encoding != "etf"*/) //todo etf format
        {
            await CloseAsync(WebSocketCloseStatus.NormalClosure,
                ((int) Constants.CloseCodes.Decode_error).ToString());
            _cancellationTokenSource.Cancel();
            return false;
        }

        if (Version != 9)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                ((int) Constants.CloseCodes.Invalid_API_version).ToString(), _cancellationToken);
            _cancellationTokenSource.Cancel();
            return false;
        }

        if (!string.IsNullOrEmpty(Compress))
        {
            if (Compress != "zlib-stream")
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    ((int) Constants.CloseCodes.Decode_error).ToString(), _cancellationToken);
                _cancellationTokenSource.Cancel();
                return false;
            }
        }

        return true;
    }

    public async Task SendAsync(Payload payload)
    {
        switch (Encoding)
        {
            case "json":
                await SendJsonAsync(payload);
                break;
            case "etf": //todo: implement
                break;
        }
    }

    private async Task SendJsonAsync(Payload payload)
    {
        string data = JsonConvert.SerializeObject(payload, JsonSerializerSettings);
        Console.WriteLine("send: " + payload.op + " " + payload.t);
        if (Static.Config.Logging.DumpGatewayEventsToFiles)
            await DumpPayloadToFile(payload, true);

        var bytes = System.Text.Encoding.UTF8.GetBytes(data);
        if (Compress == "zlib-stream")
        {
            await SendZlibStreamContinuationAsync(bytes);
        }
        else
        {
            await _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, _cancellationToken);
        }
    }

    private async Task SendZlibStreamContinuationAsync(byte[] payload)
    {
        //create streams if they dont exist yet
        _zlibInputStream ??= new();
        _zlibStream ??= new ZlibStream(_zlibInputStream, CompressionMode.Compress, CompressionLevel.Default, true);
        //write to compression stream
        await _zlibInputStream.WriteAsync(payload, 0, payload.Length, _cancellationToken);
        await _zlibInputStream.FlushAsync(_cancellationToken);
        _zlibStream.FlushMode = FlushType.None;
        await _zlibStream.FlushAsync();
        //read compressed data
        using var ms = new MemoryStream();
        await _zlibStream.CopyToAsync(ms, _cancellationToken);
        //send compressed data
        var data = ms.ToArray();
        await _webSocket.SendAsync(data, WebSocketMessageType.Binary, true, _cancellationToken);
    }

    private async Task DumpPayloadToFile(Payload payload, bool isOut)
    {
        var dir = $"event_dump/{(isOut ? "out" : "in")}/{payload.op}";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        var targetfile = $"{dir}/{Directory.GetFiles(dir).Count(x => x.EndsWith(".json"))}.json";
        await File.WriteAllTextAsync(targetfile,
            JsonConvert.SerializeObject(payload, Formatting.Indented, JsonSerializerSettings), _cancellationToken);
        Console.WriteLine($"Dumped {payload.op} to {targetfile}");
        if (Static.Config.Gateway.Debug.OpenDumpsAfterWrite && !Static.Config.Gateway.Debug.IgnoredEvents.Contains(payload.op.ToString()))
            Process.Start(Static.Config.Gateway.Debug.OpenDumpCommand.Command, Static.Config.Gateway.Debug.OpenDumpCommand.Args.Replace("$file", targetfile));
    }

    private async void HandleTimeouts()
    {
        //READY timeout
        try
        {
            await Task.Delay(1000 * 30, _cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to wait for READY timeout: {e.Message}");
        }
        
        if (!IsReady)
        {
            await CloseAsync(WebSocketCloseStatus.NormalClosure,
                ((int) Constants.CloseCodes.Unknown_error).ToString());
        }

        //Heartbeat timeout
        while (_webSocket.State == WebSocketState.Open && !_cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(Static.Config.Gateway.HeartbeatInterval, _cancellationToken);
            if (Lastheartbeat.AddSeconds(Static.Config.Gateway.HeartbeatInterval + Static.Config.Gateway.HeartbeatIntervalBuffer) < DateTime.UtcNow)
            {
                await CloseAsync(WebSocketCloseStatus.NormalClosure, ((int) Constants.CloseCodes.Session_timed_out).ToString());
            }
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        if (_disposed) return;
        _disposed = true;
        WebSockets.Remove(this);

        //dispose everything with reflection
        foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            if (field.FieldType.IsSubclassOf(typeof(IDisposable)))
            {
                var value = field.GetValue(this);
                if (value != null)
                    ((IDisposable) value).Dispose();
            }
        }
    }


    //static variables
    public static readonly Dictionary<Constants.OpCodes, IGatewayMessage> GatewayActions = new();
    public static readonly List<WebSocketInfo> WebSockets;

    //private static variables
    private static readonly Payload HelloPayload = new()
    {
        op = Constants.OpCodes.Hello,
        d = new
        {
            heartbeat_interval = Static.Config.Gateway.HeartbeatInterval
        }
    };

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    static WebSocketInfo()
    {
        WebSockets = new List<WebSocketInfo>();
    }
}