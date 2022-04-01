namespace Fosscord.API.Classes;

public class UserSettings
{
    private int afk_timeout = 3600;
    bool allow_accessibility_detection = true;
    bool animate_emoji = true;
    bool animate_stickers = false;
    bool contact_sync_enabled = false;
    bool convert_emoticons = false;
    object custom_status = null;
    bool default_guilds_restricted = false;
    bool detect_platform_accounts = false;
    bool developer_mode = true;
    bool disable_games_tab = true;
    bool enable_tts_command = false;
    bool explicit_content_filter = false;
    object friend_source_flags = new {all = true};
    bool gateway_connected = false;
    bool gif_auto_play = true;
    object guild_folders = null;
    object guild_positions = null;
    bool inline_attachment_media = true;
    bool inline_embed_media = true;
    bool message_display_compact = true;
    bool native_phone_integration_enabled = true;
    bool render_embeds = true;
    bool render_reactions = true;
    object restricted_guilds = null;
    bool show_current_game = true;
    bool stream_notifications_enabled = false;
    string locale = "en-US";
    string status = "online";
    string theme = "dark";
    int timezone_offset = 0; // TODO = timezone from request
};