﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spacebar.DbModel.Entities
{
    [Table("recipients")]
    public class Recipient
    {
        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("channel_id", TypeName = "character varying")]
        public string ChannelId { get; set; } = null!;
        [Column("user_id", TypeName = "character varying")]
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
