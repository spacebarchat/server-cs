using System.ComponentModel;
using System.Numerics;

namespace Fosscord.Shared.Enums;

public class Rights
{
    [Description("Full access rights. DANGEROUS!")]
    public static int OPERATOR = 0; // has all rights
    [Description("Can manage bots/apps regardless of permissions")]
    public static int MANAGE_APPLICATIONS = 1;
    [Description("Can manage guilds regardless of permissions")]
    public static int MANAGE_GUILDS = 2;
    [Description("Can manage messages regardless of permissions")]
    public static int MANAGE_MESSAGES = 3; // Can't see other messages but delete/edit them in channels that they can see
    [Description("Can manage ratelimits regardless of permissions")]
    public static int MANAGE_RATE_LIMITS = 4;
    [Description("Can manage custom message routing regardless of permissions")]
    public static int MANAGE_ROUTING = 5; // can create custom message routes to any channel/guild
    [Description("Can manage users regardless of permissions")]
    public static int MANAGE_TICKETS = 6; // can respond to and resolve support tickets
    public static int MANAGE_USERS = 7;
    public static int ADD_MEMBERS = 8; // can manually add any members in their guilds
    public static int BYPASS_RATE_LIMITS = 9;
    public static int CREATE_APPLICATIONS = 10;
    public static int CREATE_CHANNELS = 11; // can create guild channels or threads in the guilds that they have permission
    public static int CREATE_DMS = 12;
    public static int CREATE_DM_GROUPS = 13; // can create group DMs or custom orphan channels
    public static int CREATE_GUILDS = 14;
    public static int CREATE_INVITES = 15; // can create mass invites in the guilds that they have CREATE_INSTANT_INVITE
    public static int CREATE_ROLES = 16;
    public static int CREATE_TEMPLATES = 17;
    public static int CREATE_WEBHOOKS = 18;
    public static int JOIN_GUILDS = 19;
    public static int PIN_MESSAGES = 20;
    public static int SELF_ADD_REACTIONS = 21;
    public static int SELF_DELETE_MESSAGES = 22;
    public static int SELF_EDIT_MESSAGES = 23;
    public static int SELF_EDIT_NAME = 24;
    public static int SEND_MESSAGES = 25;
    public static int USE_ACTIVITIES = 26; // use (game) activities in voice channels (e.g. Watch together
    public static int USE_VIDEO = 27;
    public static int USE_VOICE = 28;
    public static int INVITE_USERS = 29; // can create user-specific invites in the guilds that they have INVITE_USERS
    public static int SELF_DELETE_DISABLE = 30; // can disable/delete own account
    public static int DEBTABLE = 31; // can use pay-to-use features
    public static int CREDITABLE = 32; // can receive money from monetisation related features
    public static int KICK_BAN_MEMBERS = 33;
    // can kick or ban guild or group DM members in the guilds/groups that they have KICK_MEMBERS; or BAN_MEMBERS
    public static int SELF_LEAVE_GROUPS = 34;
    // can leave the guilds or group DMs that they joined on their own (one can always leave a guild or group DMs they have been force-added)
    public static int PRESENCE = 35;
    // inverts the presence confidentiality default (OPERATOR's presence is not routed by default; others' are) for a given user
    public static int SELF_ADD_DISCOVERABLE = 36; // can mark discoverable guilds that they have permissions to mark as discoverable
    public static int MANAGE_GUILD_DIRECTORY = 37; // can change anything in the primary guild directory
    public static int POGGERS = 38; // can send confetti; screenshake; random user mention (@someone
    public static int USE_ACHIEVEMENTS = 39; // can use achievements and cheers
    public static int INITIATE_INTERACTIONS = 40; // can initiate interactions
    public static int RESPOND_TO_INTERACTIONS = 41; // can respond to interactions
    public static int SEND_BACKDATED_EVENTS = 42; // can send backdated events
    public static int USE_MASS_INVITES = 43; // added per @xnacly's request — can accept mass invites
    public static int ACCEPT_INVITES = 44; // added per @xnacly's request — can accept user-specific invites and DM requests
    public static int SELF_EDIT_FLAGS = 45; // can modify own flags
    public static int EDIT_FLAGS = 46; // can set others' flags
    public static int MANAGE_GROUPS = 47; // can manage others' groups
    public static int VIEW_SERVER_STATS = 48; // added per @chrischrome's request — can view server stats
}