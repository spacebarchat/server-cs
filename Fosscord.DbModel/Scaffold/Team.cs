using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("teams")]
    public partial class Team
    {
        public Team()
        {
            Applications = new HashSet<Application>();
            TeamMembers = new HashSet<TeamMember>();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("icon")]
        public string? Icon { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("owner_user_id")]
        public string? OwnerUserId { get; set; }

        [ForeignKey("OwnerUserId")]
        [InverseProperty("Teams")]
        public virtual User? OwnerUser { get; set; }
        [InverseProperty("Team")]
        public virtual ICollection<Application> Applications { get; set; }
        [InverseProperty("Team")]
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
