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
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("guild_id")]
        public string GuildId { get; set; } = null!;
        [Column("nick")]
        public string? Nick { get; set; }
        [Column("joined_at")]
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
        [Column("last_message_id")]
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
