namespace Spacebar.DbModel.Classes;

public class UserSettings
{
    public int AfkTimeout = 3600;
    public bool AllowAccessibilityDetection = true;
    public bool AnimateEmoji = true;
    public bool AnimateStickers = false;
    public bool ContactSyncEnabled = false;
    public bool ConvertEmoticons = false;
    public object CustomStatus = null;
    public bool DefaultGuildsRestricted = false;
    public bool DetectPlatformAccounts = false;
    public bool DeveloperMode = true;
    public bool DisableGamesTab = true;
    public bool EnableTtsCommand = false;
    public bool ExplicitContentFilter = false;
    public object FriendSourceFlags = new { all = true };
    public bool GatewayConnected = false;
    public bool GifAutoPlay = true;
    public object GuildFolders = null;
    public object GuildPositions = null;
    public bool InlineAttachmentMedia = true;
    public bool InlineEmbedMedia = true;
    public bool MessageDisplayCompact = true;
    public bool NativePhoneIntegrationEnabled = true;
    public bool RenderEmbeds = true;
    public bool RenderReactions = true;
    public object RestrictedGuilds = null;
    public bool ShowCurrentGame = true;
    public bool StreamNotificationsEnabled = false;
    public string Locale = "en-US";
    public string Status = "online";
    public string Theme = "dark";
    public int TimezoneOffset = 0; // TODO = timezone from request
};