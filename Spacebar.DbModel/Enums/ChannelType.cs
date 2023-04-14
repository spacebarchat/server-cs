namespace Spacebar.Util.Schemas;

public enum ChannelType
{
    GuildText = 0, // a text channel within a guild
    Dm = 1, // a direct message between users
    GuildVoice = 2, // a voice channel within a guild
    GroupDm = 3, // a direct message between multiple users
    GuildCategory = 4, // an organizational category that contains zero or more channels
    GuildNews = 5, // a channel that users can follow and crosspost into a guild or route
    GuildStore = 6, // a channel in which game developers can sell their things
    Encrypted = 7, // end-to-end encrypted channel
    EncryptedThread = 8, // end-to-end encrypted thread channel
    Transactional = 9, // event chain style transactional channel
    GuildNewsThread = 10, // a temporary sub-channel within a GUILD_NEWS channel
    GuildPublicThread = 11, // a temporary sub-channel within a GUILD_TEXT channel
    GuildPrivateThread = 12, // a temporary sub-channel within a GUILD_TEXT channel that is only viewable by those invited and those with the MANAGE_THREADS permission
    GuildStageVoice = 13, // a voice channel for hosting events with an audience
    Directory = 14, // guild directory listing channel
    GuildForum = 15, // forum composed of IM threads
    TicketTracker = 33, // ticket tracker, individual ticket items shall have type 12
    Kanban = 34, // confluence like kanban board
    VoicelessWhiteboard = 35, // whiteboard but without voice (whiteboard + voice is the same as stage)
    CustomStart = 64, // start custom channel types from here
    Unhandled = 255 // unhandled unowned pass-through channel type
}