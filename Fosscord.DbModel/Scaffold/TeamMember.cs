using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("team_members")]
    public partial class TeamMember
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("permissions")]
        public string Permissions { get; set; } = null!;
        [Column("team_id")]
        public string? TeamId { get; set; }
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("membership_state")]
        public int MembershipState { get; set; }

        [ForeignKey("TeamId")]
        [InverseProperty("TeamMembers")]
        public virtual Team? Team { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("TeamMembers")]
        public virtual User? User { get; set; }
    }
}
