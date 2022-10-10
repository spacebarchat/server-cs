using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosscord.DbModel.Scaffold
{
    [Table("team_members")]
    public partial class TeamMember
    {
        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("membership_state")]
        public int MembershipState { get; set; }
        [Column("permissions")]
        public string Permissions { get; set; } = null!;
        [Column("team_id", TypeName = "character varying")]
        public string? TeamId { get; set; }
        [Column("user_id", TypeName = "character varying")]
        public string? UserId { get; set; }

        [ForeignKey("TeamId")]
        [InverseProperty("TeamMembers")]
        public virtual Team? Team { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("TeamMembers")]
        public virtual User? User { get; set; }
    }
}
