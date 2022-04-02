using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("rate_limits")]
    public partial class RateLimit
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("executor_id")]
        public string ExecutorId { get; set; } = null!;
        [Column("hits")]
        public int Hits { get; set; }
        [Column("blocked")]
        public bool Blocked { get; set; }
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
