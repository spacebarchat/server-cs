using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("guilds")]
    public partial class Guild
    {
        public Guild()
        {
            Applications = new HashSet<Application>();
            Bans = new HashSet<Ban>();
            Channels = new HashSet<Channel>();
            Emojis = new HashSet<Emoji>();
            Invites = new HashSet<Invite>();
            Members = new HashSet<Member>();
            Messages = new HashSet<Message>();
            Roles = new HashSet<Role>();
            Stickers = new HashSet<Sticker>();
            Templates = new HashSet<Template>();
            VoiceStates = new HashSet<VoiceState>();
            WebhookGuilds = new HashSet<Webhook>();
            WebhookSourceGuilds = new HashSet<Webhook>();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("afk_channel_id")]
        public string? AfkChannelId { get; set; }
        [Column("afk_timeout")]
        public int? AfkTimeout { get; set; }
        [Column("banner")]
        public string? Banner { get; set; }
        [Column("default_message_notifications")]
        public int? DefaultMessageNotifications { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("discovery_splash")]
        public string? DiscoverySplash { get; set; }
        [Column("explicit_content_filter")]
        public int? ExplicitContentFilter { get; set; }
        [Column("features")]
        public string Features { get; set; } = null!;
        [Column("icon")]
        public string? Icon { get; set; }
        [Column("large")]
        public bool? Large { get; set; }
        [Column("max_members")]
        public int? MaxMembers { get; set; }
        [Column("max_presences")]
        public int? MaxPresences { get; set; }
        [Column("max_video_channel_users")]
        public int? MaxVideoChannelUsers { get; set; }
        [Column("member_count")]
        public int? MemberCount { get; set; }
        [Column("presence_count")]
        public int? PresenceCount { get; set; }
        [Column("template_id")]
        public string? TemplateId { get; set; }
        [Column("mfa_level")]
        public int? MfaLevel { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("owner_id")]
        public string? OwnerId { get; set; }
        [Column("preferred_locale")]
        public string? PreferredLocale { get; set; }
        [Column("premium_subscription_count")]
        public int? PremiumSubscriptionCount { get; set; }
        [Column("premium_tier")]
        public int? PremiumTier { get; set; }
        [Column("public_updates_channel_id")]
        public string? PublicUpdatesChannelId { get; set; }
        [Column("rules_channel_id")]
        public string? RulesChannelId { get; set; }
        [Column("region")]
        public string? Region { get; set; }
        [Column("splash")]
        public string? Splash { get; set; }
        [Column("system_channel_id")]
        public string? SystemChannelId { get; set; }
        [Column("system_channel_flags")]
        public int? SystemChannelFlags { get; set; }
        [Column("unavailable")]
        public bool? Unavailable { get; set; }
        [Column("verification_level")]
        public int? VerificationLevel { get; set; }
        [Column("welcome_screen")]
        public string WelcomeScreen { get; set; } = null!;
        [Column("widget_channel_id")]
        public string? WidgetChannelId { get; set; }
        [Column("widget_enabled")]
        public bool? WidgetEnabled { get; set; }
        [Column("nsfw_level")]
        public int? NsfwLevel { get; set; }
        [Column("nsfw")]
        public bool? Nsfw { get; set; }

        [ForeignKey("AfkChannelId")]
        [InverseProperty("GuildAfkChannels")]
        public virtual Channel? AfkChannel { get; set; }
        [ForeignKey("OwnerId")]
        [InverseProperty("Guilds")]
        public virtual User? Owner { get; set; }
        [ForeignKey("PublicUpdatesChannelId")]
        [InverseProperty("GuildPublicUpdatesChannels")]
        public virtual Channel? PublicUpdatesChannel { get; set; }
        [ForeignKey("RulesChannelId")]
        [InverseProperty("GuildRulesChannels")]
        public virtual Channel? RulesChannel { get; set; }
        [ForeignKey("SystemChannelId")]
        [InverseProperty("GuildSystemChannels")]
        public virtual Channel? SystemChannel { get; set; }
        [ForeignKey("TemplateId")]
        [InverseProperty("Guilds")]
        public virtual Template? Template { get; set; }
        [ForeignKey("WidgetChannelId")]
        [InverseProperty("GuildWidgetChannels")]
        public virtual Channel? WidgetChannel { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Application> Applications { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Ban> Bans { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Channel> Channels { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Emoji> Emojis { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Invite> Invites { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Member> Members { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Message> Messages { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Role> Roles { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Sticker> Stickers { get; set; }
        [InverseProperty("SourceGuild")]
        public virtual ICollection<Template> Templates { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<VoiceState> VoiceStates { get; set; }
        [InverseProperty("Guild")]
        public virtual ICollection<Webhook> WebhookGuilds { get; set; }
        [InverseProperty("SourceGuild")]
        public virtual ICollection<Webhook> WebhookSourceGuilds { get; set; }
    }
}
