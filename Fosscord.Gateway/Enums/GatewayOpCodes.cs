namespace Fosscord.Static.Enums;

public enum GatewayOpCodes
{
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