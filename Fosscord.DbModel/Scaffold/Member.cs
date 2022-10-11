using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("members")]
    [Index("Id", "GuildId", Name = "IDX_bb2bf9386ac443afbbbf9f12d3", IsUnique = true)]
    public partial class Member
    {
        public Member()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        [Column("index")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Index { get; set; }
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("guild_id", TypeName = "character varying")]
        public string GuildId { get; set; } = null!;
        [Column("nick", TypeName = "character varying")]
        public string? Nick { get; set; }
        [Column("joined_at", TypeName = "timestamp without time zone")]
        public DateTime JoinedAt { get; set; }
        [Column("deaf")]
        public bool Deaf { get; set; }
        [Column("mute")]
        public bool Mute { get; set; }
        [Column("pending")]
        public bool Pending { get; set; }
        [Column("settings", TypeName = "jsonb")]
        public UserChannelSettings Settings { get; set; } = null!;
        [Column("last_message_id", TypeName = "character varying")]
        public string? LastMessageId { get; set; }
        [Column("joined_by", TypeName = "character varying")]
        public string? JoinedBy { get; set; }
        [Column("premium_since", TypeName = "timestamp without time zone")]
        public DateTime? PremiumSince { get; set; }
        [Column("avatar", TypeName = "character varying")]
        public string? Avatar { get; set; }
        [Column("banner", TypeName = "character varying")]
        public string? Banner { get; set; }
        [Column("bio", TypeName = "character varying")]
        public string Bio { get; set; } = null!;
        [Column("communication_disabled_until", TypeName = "timestamp without time zone")]
        public DateTime? CommunicationDisabledUntil { get; set; }

        [ForeignKey("GuildId")]
        [InverseProperty("Members")]
        public virtual Guild Guild { get; set; } = null!;
        [ForeignKey("Id")]
        [InverseProperty("Members")]
        public virtual User IdNavigation { get; set; } = null!;

        [ForeignKey("Index")]
        [InverseProperty("Indices")]
        public virtual ICollection<Role> Roles { get; set; }
    }

    public class UserChannelSettings
    {
        //{"channel_overrides":[],"message_notifications":0,"mobile_push":true,"muted":false,"suppress_everyone":false,"suppress_roles":false,"version":0}
        public int Version { get; set; }= 0;
        public int MessageNotifications { get; set; } = 0;
        public bool MobilePush { get; set; } = true;
        public bool Muted { get; set; } = false;
        public bool SuppressEveryone { get; set; } = false;
        public bool SuppressRoles { get; set; } = false;
        public object[] ChannelOverrides { get; set; } = Array.Empty<object>();
    } 
}
