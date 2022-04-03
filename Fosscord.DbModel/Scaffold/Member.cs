using System;
using System.Collections.Generic;
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
            Messages = new HashSet<Message>();
            Roles = new HashSet<Role>();
        }

        [Key]
        [Column("index")]
        public int Index { get; set; }
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("guild_id", TypeName = "character varying")]
        public string GuildId { get; set; } = null!;
        [Column("nick", TypeName = "character varying")]
        public string? Nick { get; set; }
        [Column("joined_at", TypeName = "timestamp without time zone")]
        public DateTime JoinedAt { get; set; }
        [Column("premium_since")]
        public int? PremiumSince { get; set; }
        [Column("deaf")]
        public bool Deaf { get; set; }
        [Column("mute")]
        public bool Mute { get; set; }
        [Column("pending")]
        public bool Pending { get; set; }
        [Column("settings")]
        public string Settings { get; set; } = null!;
        [Column("last_message_id", TypeName = "character varying")]
        public string? LastMessageId { get; set; }

        [ForeignKey("GuildId")]
        [InverseProperty("Members")]
        public virtual Guild Guild { get; set; } = null!;
        [ForeignKey("Id")]
        [InverseProperty("Members")]
        public virtual User IdNavigation { get; set; } = null!;
        [InverseProperty("Member")]
        public virtual ICollection<Message> Messages { get; set; }

        [ForeignKey("Index")]
        [InverseProperty("Indices")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
