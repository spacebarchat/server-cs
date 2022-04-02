using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("audit_logs")]
    public partial class AuditLog
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("options")]
        public string? Options { get; set; }
        [Column("changes")]
        public string Changes { get; set; } = null!;
        [Column("reason")]
        public string? Reason { get; set; }
        [Column("target_id")]
        public string? TargetId { get; set; }
        [Column("action_type")]
        public int ActionType { get; set; }

        [ForeignKey("TargetId")]
        [InverseProperty("AuditLogTargets")]
        public virtual User? Target { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("AuditLogUsers")]
        public virtual User? User { get; set; }
    }
}
