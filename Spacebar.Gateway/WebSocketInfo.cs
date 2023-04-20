using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Reflection.Metadata;
using ArcaneLibs;
using Spacebar.ConfigModel;
using Spacebar.Gateway.Events;
using Spacebar.Static.Classes;
using Spacebar.Static.Enums;
using Spacebar.Util.Rewrites;
using Ionic.Zlib;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Spacebar.Static.Utilities;

namespace Spacebar.Gateway.Models;

public class WebSocketInfo : IDisposable
{
    private readonly GatewayMessageTypeService _gatewayMessageTypeService;
    private bool _disposed = false;
    public int Version;
    public string UserId;
    public string SessionId = RandomStringGenerator.Generate(32);
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

    public WebSocketInfo(GatewayMessageTypeService gatewayMessageTypeService)
    {
        _gatewayMessageTypeService = gatewayMessageTypeService;
        _cancellationToken = _cancellationTokenSource.Token;
    }

    public WebSocketInfo WithSettings(string encoding, int v, string compress = "")
    {
        Encoding = encoding;
        Version = v;
        Compress = compress;
        return this;
    }

    public async Task AcceptWebSocketAsync(WebSocketManager manager)
    {
        _webSocket = await manager.AcceptWebSocketAsync();
        var isValid = await ValidateWebsocketParameters();
        if (isValid)
        {
            Console.WriteLine($"Accepted WS connection: {SessionId}");
            await SendAsync(HelloPayload);

            Console.WriteLine($"Starting receive loop for {SessionId}");
            try
            {
                //RunReceiveLoop();
                HandleTimeouts();
                await RunReceiveLoop();
            }
            catch (WebSocketException e)
            {
                Console.WriteLine($"Connection closed: {e.Message}");
            }

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
        var messageBuffer = WebSocket.CreateClientBuffer(65535, 65535);
        WebSocketReceiveResult result;
        while (_webSocket.State == WebSocketState.Open && !_cancellationToken.IsCancellationRequested)
        {
            using var ms = new MemoryStream();
            
            do
            {
                result = await _webSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
            } while (!result.EndOfMessage && !_cancellationToken.IsCancellationRequested && !_webSocket.CloseStatus.HasValue);
            
            var msgString = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            //Console.WriteLine($"{result.MessageType}\n{msgString}");
            var message = JsonConvert.DeserializeObject<GatewayPayload>(msgString);
            if (message != null && !string.IsNullOrEmpty(msgString))
            {
                await DumpPayloadToFile(message, false);
                Console.WriteLine(
                    $"[{SessionId}:JSON] <- {message.op} {message.t} ({ArcaneLibs.Util.BytesToString(ms.Length)})");
                if (_gatewayMessageTypeService.GatewayActions.ContainsKey(message.op))
                {
                    try
                    {
                        await _gatewayMessageTypeService.GatewayActions[message.op].Invoke(message, this);
                    }
                    catch (Exception e) //safely disconnect
                    {
                        Console.WriteLine($"Failed to execute GatewayAction\n {e}");
                        if (e is WebSocketException && e.InnerException is ObjectDisposedException)
                        {
                            Console.WriteLine("Object disposed exception, ignoring!");
                            return;
                        }

                        await CloseAsync(WebSocketCloseStatus.NormalClosure,
                            ((int)GatewayCloseCodes.UnknownError).ToString());
                        _cancellationTokenSource.Cancel();
                    }
                }
                else
                {
                    Console.WriteLine("Unknown OP code!");
                    Console.WriteLine($"Client called for missing OP code {message.op}");
                    //write to file
                    if (!Directory.Exists("unknown_events")) Directory.CreateDirectory("unknown_events");
                    if (!Directory.Exists($"unknown_events/{message.op}"))
                        Directory.CreateDirectory($"unknown_events/{message.op}");
                    await File.WriteAllTextAsync(
                        $"unknown_events/{message.op}/{Directory.GetFiles($"unknown_events/{message.op}").Length}.json",
                        JsonConvert.SerializeObject((object)JsonConvert.DeserializeObject<dynamic>(msgString),
                            Formatting.Indented), _cancellationToken);
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
                ((int)GatewayCloseCodes.DecodeError).ToString());
            _cancellationTokenSource.Cancel();
            return false;
        }

        if (Version != 9)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                ((int)GatewayCloseCodes.InvalidApiVersion).ToString(), _cancellationToken);
            _cancellationTokenSource.Cancel();
            return false;
        }

        if (!string.IsNullOrEmpty(Compress) && Compress != "zlib-stream")
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                ((int)GatewayCloseCodes.DecodeError).ToString(), _cancellationToken);
            _cancellationTokenSource.Cancel();
            return false;
        }

        return true;
    }

    public async Task SendAsync(GatewayPayload payload)
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

    private async Task SendJsonAsync(GatewayPayload payload)
    {
        var data = JsonConvert.SerializeObject(payload, JsonSerializerSettings);
        if (Config.Instance.Gateway.Debug.DumpGatewayEventsToFiles)
            await DumpPayloadToFile(payload, true);

        var bytes = System.Text.Encoding.UTF8.GetBytes(data);
        Console.WriteLine(
            $"[{SessionId}:JSON] -> {payload.op} {payload.t} ({ArcaneLibs.Util.BytesToString(bytes.Length)})");
        if (Compress == "zlib-stream")
        {
            await SendZlibStreamContinuationAsync(bytes);
        }
        else
        {
            if (_webSocket.State == WebSocketState.Open)
                try
                {
                    await _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, _cancellationToken);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine($"Could not send message: {e.Message}");
                }
        }
    }

    private async Task SendZlibStreamContinuationAsync(byte[] payload)
    {
        //create streams if they dont exist yet
        _zlibInputStream ??= new MemoryStream();
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

    private async Task DumpPayloadToFile(GatewayPayload payload, bool isOut)
    {
        if (!Config.Instance.Gateway.Debug.DumpGatewayEventsToFiles) return;
        await WriteObjectToFile(payload, $"event_dump/{SessionId}/{(isOut ? "out" : "in")}/{payload.op}", "#.json");
        await WriteObjectToFile(payload, $"event_dump/{SessionId}/all", $"#-{(isOut ? "out" : "in")}.json");
    }

    private async Task WriteObjectToFile(object obj, string dir, string filenamefmt)
    {
        //check if parent dirs exist
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var fileCount = Directory.GetFiles(dir).Count(x => x.EndsWith(".json"));
        await File.WriteAllTextAsync(
            $"{dir}/{filenamefmt.Replace("#", fileCount.ToString())}",
            JsonConvert.SerializeObject(obj, Formatting.Indented, JsonSerializerSettings), _cancellationToken);
    }

    private async Task HandleTimeouts()
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
            await CloseAsync(WebSocketCloseStatus.NormalClosure,
                ((int)GatewayCloseCodes.UnknownError).ToString());

        //Heartbeat timeout
        while (_webSocket.State == WebSocketState.Open && !_cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(Config.Instance.Gateway.HeartbeatInterval, _cancellationToken);
            }
            catch (TaskCanceledException e)
            {
                //Console.WriteLine($"[{SessionId}] Stopping heartbeat timeout task: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to wait for heartbeat timeout: {e.Message}");
            }

            if (Lastheartbeat.AddSeconds(Config.Instance.Gateway.HeartbeatInterval +
                                         Config.Instance.Gateway.HeartbeatIntervalBuffer) <
                DateTime.UtcNow)
                await CloseAsync(WebSocketCloseStatus.NormalClosure,
                    ((int)GatewayCloseCodes.SessionTimedOut).ToString());
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
            if (field.FieldType.IsSubclassOf(typeof(IDisposable)))
            {
                var value = field.GetValue(this);
                if (value != null)
                    ((IDisposable)value).Dispose();
            }
    }

    //static variables
    public static readonly List<WebSocketInfo> WebSockets;

    //private static variables
    private static readonly GatewayPayload HelloPayload = new()
    {
        op = GatewayOpCodes.Hello,
        d = new
        {
            heartbeat_interval = Config.Instance.Gateway.HeartbeatInterval
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