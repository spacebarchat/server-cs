using System.ComponentModel;

namespace Fosscord.Shared.Enums;

public static class Rights
{
    [Description("Full access rights. DANGEROUS!")]
    public const int OPERATOR = 0; // has all rights
    [Description("Can manage bots/apps regardless of permissions")]
    public const int MANAGE_APPLICATIONS = 1;
    [Description("Can manage guilds regardless of permissions")]
    public const int MANAGE_GUILDS = 2;
    [Description("Can manage messages regardless of permissions")]
    public const int MANAGE_MESSAGES = 3; // Can't see other messages but delete/edit them in channels that they can see
    [Description("Can manage ratelimits regardless of permissions")]
    public const int MANAGE_RATE_LIMITS = 4;
    [Description("Can manage custom message routing regardless of permissions")]
    public const int MANAGE_ROUTING = 5; // can create custom message routes to any channel/guild
    [Description("Can respond to and resolve support tickets")]
    public const int MANAGE_TICKETS = 6; // can respond to and resolve support tickets
    [Description("Can manage users regardless of permissions")]
    public const int MANAGE_USERS = 7;
    [Description("Can manually add members to guilds")]
    public const int ADD_MEMBERS = 8;
    public const int BYPASS_RATE_LIMITS = 9;
    public const int CREATE_APPLICATIONS = 10;
    public const int CREATE_CHANNELS = 11; // can create guild channels or threads in the guilds that they have permission
    public const int CREATE_DMS = 12;
    public const int CREATE_DM_GROUPS = 13; // can create group DMs or custom orphan channels
    public const int CREATE_GUILDS = 14;
    public const int CREATE_INVITES = 15; // can create mass invites in the guilds that they have CREATE_INSTANT_INVITE
    public const int CREATE_ROLES = 16;
    public const int CREATE_TEMPLATES = 17;
    public const int CREATE_WEBHOOKS = 18;
    public const int JOIN_GUILDS = 19;
    public const int PIN_MESSAGES = 20;
    public const int SELF_ADD_REACTIONS = 21;
    public const int SELF_DELETE_MESSAGES = 22;
    public const int SELF_EDIT_MESSAGES = 23;
    public const int SELF_EDIT_NAME = 24;
    public const int SEND_MESSAGES = 25;
    public const int USE_ACTIVITIES = 26; // use (game) activities in voice channels (e.g. Watch together
    public const int USE_VIDEO = 27;
    public const int USE_VOICE = 28;
    public const int INVITE_USERS = 29; // can create user-specific invites in the guilds that they have INVITE_USERS
    public const int SELF_DELETE_DISABLE = 30; // can disable/delete own account
    public const int DEBTABLE = 31; // can use pay-to-use features
    public const int CREDITABLE = 32; // can receive money from monetisation related features
    public const int KICK_BAN_MEMBERS = 33;
    // can kick or ban guild or group DM members in the guilds/groups that they have KICK_MEMBERS; or BAN_MEMBERS
    public const int SELF_LEAVE_GROUPS = 34;
    // can leave the guilds or group DMs that they joined on their own (one can always leave a guild or group DMs they have been force-added)
    public const int PRESENCE = 35;
    // inverts the presence confidentiality default (OPERATOR's presence is not routed by default; others' are) for a given user
    public const int SELF_ADD_DISCOVERABLE = 36; // can mark discoverable guilds that they have permissions to mark as discoverable
    public const int MANAGE_GUILD_DIRECTORY = 37; // can change anything in the primary guild directory
    public const int POGGERS = 38; // can send confetti; screenshake; random user mention (@someone
    public const int USE_ACHIEVEMENTS = 39; // can use achievements and cheers
    public const int INITIATE_INTERACTIONS = 40; // can initiate interactions
    public const int RESPOND_TO_INTERACTIONS = 41; // can respond to interactions
    public const int SEND_BACKDATED_EVENTS = 42; // can send backdated events
    public const int USE_MASS_INVITES = 43; // added per @xnacly's request — can accept mass invites
    public const int ACCEPT_INVITES = 44; // added per @xnacly's request — can accept user-specific invites and DM requests
    public const int SELF_EDIT_FLAGS = 45; // can modify own flags
    public const int EDIT_FLAGS = 46; // can set others' flags
    public const int MANAGE_GROUPS = 47; // can manage others' groups
    public const int VIEW_SERVER_STATS = 48; // added per @chrischrome's request — can view server stats
}