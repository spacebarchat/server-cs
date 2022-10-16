namespace Fosscord.Gateway.Models;

public class Constants
{
    public enum OpCodes {
        Dispatch = 0,
        Heartbeat = 1,
        Identify = 2,
        PresenceUpdate = 3,
        VoiceStateUpdate = 4,
        VoiceServerPing = 5, // ? What is opcode 5?
        Resume = 6,
        Reconnect = 7,
        RequestGuildMembers = 8,
        InvalidSession = 9,
        Hello = 10,
        HeartbeatAck = 11,
        GuildSync = 12,
        DmUpdate = 13,
        LazyRequest = 14,
        LobbyConnect = 15,
        LobbyDisconnect = 16,
        LobbyVoiceStatesUpdate = 17,
        StreamCreate = 18,
        StreamDelete = 19,
        StreamWatch = 20,
        StreamPing = 21,
        StreamSetPaused = 22,
        RequestApplicationCommands = 24,
    }
    public enum CloseCodes {
        Unknown_error = 4000,
        Unknown_opcode,
        Decode_error,
        Not_authenticated,
        Authentication_failed,
        Already_authenticated,
        Invalid_session,
        Invalid_seq,
        Rate_limited,
        Session_timed_out,
        Invalid_shard,
        Sharding_required,
        Invalid_API_version,
        Invalid_intent,
        Disallowed_intent,
    }
}

public class Payload
{
    public Constants.OpCodes op { get; set; }
    public int? s { get; set; }
    public string? t { get; set; }
    public object? d { get; set; }
}