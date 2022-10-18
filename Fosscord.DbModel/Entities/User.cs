using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fosscord.ConfigModel;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Entities
{
    [Table("users")]
    [Index("SettingsId", Name = "UQ_76ba283779c8441fd5ff819c8cf", IsUnique = true)]
    public class User
    {
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
        public bool Desktop { get; set; } = false;

        [Column("mobile")]
        public bool Mobile { get; set; } = false;

        [Column("premium")]
        public bool Premium { get; set; } = true;

        [Column("premium_type")]
        public int PremiumType { get; set; } = 2;

        [Column("bot")]
        public bool Bot { get; set; } = false;

        [Column("bio", TypeName = "character varying")]
        public string? Bio { get; set; } = "";

        [Column("system")]
        public bool System { get; set; } = false;

        [Column("nsfw_allowed")]
        public bool NsfwAllowed { get; set; } = true;

        [Column("mfa_enabled")]
        public bool? MfaEnabled { get; set; } = false;
        [Column("totp_secret", TypeName = "character varying")]
        public string? TotpSecret { get; set; }
        [Column("totp_last_ticket", TypeName = "character varying")]
        public string? TotpLastTicket { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("premium_since", TypeName = "timestamp without time zone")]
        public DateTime? PremiumSince { get; set; }

        [Column("verified")]
        public bool Verified { get; set; } = true;

        [Column("disabled")]
        public bool Disabled { get; set; } = false;

        [Column("deleted")]
        public bool Deleted { get; set; } = false;
        [Column("email", TypeName = "character varying")]
        public string? Email { get; set; }
        [Column("flags", TypeName = "character varying")]
        public string Flags { get; set; } = "0";

        [Column("public_flags")]
        public int PublicFlags { get; set; } = 0;

        [Column("rights")]
        public BitArray Rights { get; set; } = Config.Instance.Security.Register.DefaultRights;
        [Column("data", TypeName = "jsonb")]
        public UserData Data { get; set; } = null!;
        [Column("fingerprints")]
        public string Fingerprints { get; set; } = "";
        [Column("extended_settings")]
        public string ExtendedSettings { get; set; } = "";
        [Column("settingsId", TypeName = "character varying")]
        public string? SettingsId { get; set; }

        [ForeignKey("SettingsId")]
        [InverseProperty("User")]
        public virtual UserSetting? Settings { get; set; }
        [InverseProperty("BotUser")]
        public virtual Application? ApplicationBotUser { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Application> ApplicationOwners { get; set; } = new HashSet<Application>();

        [InverseProperty("Target")]
        public virtual ICollection<AuditLog> AuditLogTargets { get; set; } = new HashSet<AuditLog>();

        [InverseProperty("User")]
        public virtual ICollection<AuditLog> AuditLogUsers { get; set; } = new HashSet<AuditLog>();

        [InverseProperty("User")]
        public virtual ICollection<BackupCode> BackupCodes { get; set; } = new HashSet<BackupCode>();

        [InverseProperty("Executor")]
        public virtual ICollection<Ban> BanExecutors { get; set; } = new HashSet<Ban>();

        [InverseProperty("User")]
        public virtual ICollection<Ban> BanUsers { get; set; } = new HashSet<Ban>();

        [InverseProperty("Owner")]
        public virtual ICollection<Channel> Channels { get; set; } = new HashSet<Channel>();

        [InverseProperty("User")]
        public virtual ICollection<ConnectedAccount> ConnectedAccounts { get; set; } = new HashSet<ConnectedAccount>();

        [InverseProperty("User")]
        public virtual ICollection<Emoji> Emojis { get; set; } = new HashSet<Emoji>();

        [InverseProperty("Owner")]
        public virtual ICollection<Guild> Guilds { get; set; } = new HashSet<Guild>();

        [InverseProperty("Inviter")]
        public virtual ICollection<Invite> InviteInviters { get; set; } = new HashSet<Invite>();

        [InverseProperty("TargetUser")]
        public virtual ICollection<Invite> InviteTargetUsers { get; set; } = new HashSet<Invite>();

        [InverseProperty("IdNavigation")]
        public virtual ICollection<Member> Members { get; set; } = new HashSet<Member>();

        [InverseProperty("Author")]
        public virtual ICollection<Message> MessageAuthors { get; set; } = new HashSet<Message>();

        [InverseProperty("Member")]
        public virtual ICollection<Message> MessageMembers { get; set; } = new HashSet<Message>();

        [InverseProperty("Owner")]
        public virtual ICollection<Note> NoteOwners { get; set; } = new HashSet<Note>();

        [InverseProperty("Target")]
        public virtual ICollection<Note> NoteTargets { get; set; } = new HashSet<Note>();

        [InverseProperty("User")]
        public virtual ICollection<ReadState> ReadStates { get; set; } = new HashSet<ReadState>();

        [InverseProperty("User")]
        public virtual ICollection<Recipient> Recipients { get; set; } = new HashSet<Recipient>();

        [InverseProperty("From")]
        public virtual ICollection<Relationship> RelationshipFroms { get; set; } = new HashSet<Relationship>();

        [InverseProperty("To")]
        public virtual ICollection<Relationship> RelationshipTos { get; set; } = new HashSet<Relationship>();

        [InverseProperty("User")]
        public virtual ICollection<Session> Sessions { get; set; } = new HashSet<Session>();

        [InverseProperty("User")]
        public virtual ICollection<Sticker> Stickers { get; set; } = new HashSet<Sticker>();

        [InverseProperty("User")]
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new HashSet<TeamMember>();

        [InverseProperty("OwnerUser")]
        public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();

        [InverseProperty("Creator")]
        public virtual ICollection<Template> Templates { get; set; } = new HashSet<Template>();

        [InverseProperty("User")]
        public virtual ICollection<VoiceState> VoiceStates { get; set; } = new HashSet<VoiceState>();

        [InverseProperty("User")]
        public virtual ICollection<Webhook> Webhooks { get; set; } = new HashSet<Webhook>();

        [ForeignKey("UsersId")]
        [InverseProperty("Users")]
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }

    public class UserData
    {
        public string Hash { get; set; }
        public DateTime ValidTokensSince { get; set; }
    }
}
