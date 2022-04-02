using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("bans")]
    public partial class Ban
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("guild_id")]
        public string? GuildId { get; set; }
        [Column("executor_id")]
        public string? ExecutorId { get; set; }
        [Column("ip")]
        public string Ip { get; set; } = null!;
        [Column("reason")]
        public string? Reason { get; set; }

        [ForeignKey("ExecutorId")]
        [InverseProperty("BanExecutors")]
        public virtual User? Executor { get; set; }
        [ForeignKey("GuildId")]
        [InverseProperty("Bans")]
        public virtual Guild? Guild { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BanUsers")]
        public virtual User? User { get; set; }
    }
}
