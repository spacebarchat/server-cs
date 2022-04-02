namespace Fosscord.Gateway.Models;

public class Websocket
{
    public int version;
    public string user_id;
    public string session_id;
    public string encoding;
    public string? compress;
    public long? shard_count;
    public long? shard_id;
    //public deflate?: Deflate; todo:????????
    public DateTime Lastheartbeat;
    public bool is_ready;
    public Intents intents;
    public int sequence;
    public Dictionary<string, object> permissions; //todo: permissions
    public Dictionary<string, Action> events;
    public Dictionary<string, Action> member_events;
    public object listen_options;
    public CancellationToken CancellationToken;
}