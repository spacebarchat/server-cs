using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("sessions")]
    public partial class Session
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("session_id")]
        public string SessionId { get; set; } = null!;
        [Column("client_info")]
        public string ClientInfo { get; set; } = null!;
        [Column("status")]
        public string Status { get; set; } = null!;
        [Column("activities")]
        public string? Activities { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Sessions")]
        public virtual User? User { get; set; }
    }
}
