using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("users")]
    [Index("SettingsId", Name = "UQ_76ba283779c8441fd5ff819c8cf", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            ApplicationOwners = new HashSet<Application>();
            AuditLogTargets = new HashSet<AuditLog>();
            AuditLogUsers = new HashSet<AuditLog>();
            BackupCodes = new HashSet<BackupCode>();
            BanExecutors = new HashSet<Ban>();
            BanUsers = new HashSet<Ban>();
            Channels = new HashSet<Channel>();
            ConnectedAccounts = new HashSet<ConnectedAccount>();
            Emojis = new HashSet<Emoji>();
            Guilds = new HashSet<Guild>();
            InviteInviters = new HashSet<Invite>();
            InviteTargetUsers = new HashSet<Invite>();
            Members = new HashSet<Member>();
            MessageAuthors = new HashSet<Message>();
            MessageMembers = new HashSet<Message>();
            NoteOwners = new HashSet<Note>();
            NoteTargets = new HashSet<Note>();
            ReadStates = new HashSet<ReadState>();
            Recipients = new HashSet<Recipient>();
            RelationshipFroms = new HashSet<Relationship>();
            RelationshipTos = new HashSet<Relationship>();
            Sessions = new HashSet<Session>();
            Stickers = new HashSet<Sticker>();
            TeamMembers = new HashSet<TeamMember>();
            Teams = new HashSet<Team>();
            Templates = new HashSet<Template>();
            VoiceStates = new HashSet<VoiceState>();
            Webhooks = new HashSet<Webhook>();
            Messages = new HashSet<Message>();
        }

        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("username", TypeName = "character varying")]
        public string Username { get; set; } = null!;
        [Column("discriminator", TypeName = "character varying")]
        public string Discriminator { get; set; } = null!;
        [Column("avatar", TypeName = "character varying")]
        public string? Avatar { get; set; }
        [Column("accent_color")]
        public int? AccentColor { get; set; }
        [Column("banner", TypeName = "character varying")]
        public string? Banner { get; set; }
        [Column("phone", TypeName = "character varying")]
        public string? Phone { get; set; }
        [Column("desktop")]
        public bool Desktop { get; set; }
        [Column("mobile")]
        public bool Mobile { get; set; }
        [Column("premium")]
        public bool Premium { get; set; }
        [Column("premium_type")]
        public int PremiumType { get; set; }
        [Column("bot")]
        public bool Bot { get; set; }
        [Column("bio", TypeName = "character varying")]
        public string? Bio { get; set; }
        [Column("system")]
        public bool System { get; set; }
        [Column("nsfw_allowed")]
        public bool NsfwAllowed { get; set; }
        [Column("mfa_enabled")]
        public bool? MfaEnabled { get; set; }
        [Column("totp_secret", TypeName = "character varying")]
        public string? TotpSecret { get; set; }
        [Column("totp_last_ticket", TypeName = "character varying")]
        public string? TotpLastTicket { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("premium_since", TypeName = "timestamp without time zone")]
        public DateTime? PremiumSince { get; set; }
        [Column("verified")]
        public bool Verified { get; set; }
        [Column("disabled")]
        public bool Disabled { get; set; }
        [Column("deleted")]
        public bool Deleted { get; set; }
        [Column("email", TypeName = "character varying")]
        public string? Email { get; set; }
        [Column("flags", TypeName = "character varying")]
        public string Flags { get; set; } = null!;
        [Column("public_flags")]
        public int PublicFlags { get; set; }
        [Column("rights")]
        public ulong Rights { get; set; }
        [Column("data")]
        public string Data { get; set; } = null!;
        [Column("fingerprints")]
        public string Fingerprints { get; set; } = null!;
        [Column("extended_settings")]
        public string ExtendedSettings { get; set; } = null!;
        [Column("settingsId", TypeName = "character varying")]
        public string? SettingsId { get; set; }

        [ForeignKey("SettingsId")]
        [InverseProperty("User")]
        public virtual UserSetting? Settings { get; set; }
        [InverseProperty("BotUser")]
        public virtual Application? ApplicationBotUser { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Application> ApplicationOwners { get; set; }
        [InverseProperty("Target")]
        public virtual ICollection<AuditLog> AuditLogTargets { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AuditLog> AuditLogUsers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BackupCode> BackupCodes { get; set; }
        [InverseProperty("Executor")]
        public virtual ICollection<Ban> BanExecutors { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Ban> BanUsers { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Channel> Channels { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ConnectedAccount> ConnectedAccounts { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Emoji> Emojis { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Guild> Guilds { get; set; }
        [InverseProperty("Inviter")]
        public virtual ICollection<Invite> InviteInviters { get; set; }
        [InverseProperty("TargetUser")]
        public virtual ICollection<Invite> InviteTargetUsers { get; set; }
        [InverseProperty("IdNavigation")]
        public virtual ICollection<Member> Members { get; set; }
        [InverseProperty("Author")]
        public virtual ICollection<Message> MessageAuthors { get; set; }
        [InverseProperty("Member")]
        public virtual ICollection<Message> MessageMembers { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Note> NoteOwners { get; set; }
        [InverseProperty("Target")]
        public virtual ICollection<Note> NoteTargets { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ReadState> ReadStates { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Recipient> Recipients { get; set; }
        [InverseProperty("From")]
        public virtual ICollection<Relationship> RelationshipFroms { get; set; }
        [InverseProperty("To")]
        public virtual ICollection<Relationship> RelationshipTos { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Session> Sessions { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Sticker> Stickers { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
        [InverseProperty("OwnerUser")]
        public virtual ICollection<Team> Teams { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<Template> Templates { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<VoiceState> VoiceStates { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Webhook> Webhooks { get; set; }

        [ForeignKey("UsersId")]
        [InverseProperty("Users")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
