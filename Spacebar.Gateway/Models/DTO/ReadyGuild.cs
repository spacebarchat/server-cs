using Spacebar.DbModel.Entities;

namespace Spacebar.Gateway.Models.DTO;

public class ReadyGuildDTO
{
    public ApplicationCommandCounts application_command_counts;
    public Channel[] channels;
    public string data_mode; // what is this
    public Emoji[] emojis;
    public object[] guild_scheduled_events; // TODO
    public string id;
    public bool? large;
    public bool? lazy;
    public int member_count;
    public Member[] members;
    public int premium_subscription_count;
    public ReadyGuildProperties properties;
    public Role[] roles;
    public object[] stage_instances;
    public Sticker[] stickers;
    public object[] threads;
    public string version;
}

public class ReadyGuildProperties
{
    public string name;
    public string? description;
    public string? icon;
    public string? splash;
    public string? banner;
    public string[] features;
    public string? preferred_locale;
    public string? owner_id;
    public string? application_id;
    public string? afk_channel_id;
    public int? afk_timeout;
    public string? system_channel_id;
    public int? verification_level;
    public int? explicit_content_filter;
    public int? default_message_notifications;
    public int? mfa_level;
    public string? vanity_url_code;
    public int? premium_tier;
    public bool premium_progress_bar_enable;
    public int? system_channel_flags;
    public string? discovery_splash;
    public string? rules_channel_id;
    public string? public_updates_channel_id;
    public int? max_video_channel_users;
    public int? max_members;
    public int? nsfw_level;
    public object? hub_type; // ????
}

public class ApplicationCommandCounts
{
    public int one = 0;
    public int two = 0;
    public int three = 0;
}