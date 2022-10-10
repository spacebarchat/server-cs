using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosscord.DbModel.Scaffold
{
    [Table("sessions")]
    public partial class Session
    {
        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("user_id", TypeName = "character varying")]
        public string? UserId { get; set; }
        [Column("session_id", TypeName = "character varying")]
        public string SessionId { get; set; } = null!;
        [Column("activities")]
        public string? Activities { get; set; }
        [Column("client_info")]
        public string ClientInfo { get; set; } = null!;
        [Column("status", TypeName = "character varying")]
        public string Status { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("Sessions")]
        public virtual User? User { get; set; }
    }
}
