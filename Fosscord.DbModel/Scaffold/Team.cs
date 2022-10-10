using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("icon", TypeName = "character varying")]
        public string? Icon { get; set; }
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; } = null!;
        [Column("owner_user_id", TypeName = "character varying")]
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
