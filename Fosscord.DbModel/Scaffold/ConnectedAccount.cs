using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("connected_accounts")]
    public partial class ConnectedAccount
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("access_token")]
        public string AccessToken { get; set; } = null!;
        [Column("friend_sync")]
        public bool FriendSync { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("revoked")]
        public bool Revoked { get; set; }
        [Column("show_activity")]
        public bool ShowActivity { get; set; }
        [Column("type")]
        public string Type { get; set; } = null!;
        [Column("verifie")]
        public bool Verifie { get; set; }
        [Column("visibility")]
        public int Visibility { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ConnectedAccounts")]
        public virtual User? User { get; set; }
    }
}
