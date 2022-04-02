using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("recipients")]
    public partial class Recipient
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("channel_id")]
        public string ChannelId { get; set; } = null!;
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [Column("closed")]
        public bool Closed { get; set; }

        [ForeignKey("ChannelId")]
        [InverseProperty("Recipients")]
        public virtual Channel Channel { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Recipients")]
        public virtual User User { get; set; } = null!;
    }
}
