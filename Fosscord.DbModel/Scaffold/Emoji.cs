using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("emojis")]
    public partial class Emoji
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("animated")]
        public bool Animated { get; set; }
        [Column("available")]
        public bool Available { get; set; }
        [Column("guild_id")]
        public string GuildId { get; set; } = null!;
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("managed")]
        public bool Managed { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("require_colons")]
        public bool RequireColons { get; set; }
        [Column("roles")]
        public string Roles { get; set; } = null!;

        [ForeignKey("GuildId")]
        [InverseProperty("Emojis")]
        public virtual Guild Guild { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Emojis")]
        public virtual User? User { get; set; }
    }
}
