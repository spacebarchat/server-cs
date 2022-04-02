using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("invites")]
    public partial class Invite
    {
        [Key]
        [Column("code")]
        public string Code { get; set; } = null!;
        [Column("temporary")]
        public bool Temporary { get; set; }
        [Column("uses")]
        public int Uses { get; set; }
        [Column("max_uses")]
        public int MaxUses { get; set; }
        [Column("max_age")]
        public int MaxAge { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }
        [Column("guild_id")]
        public string? GuildId { get; set; }
        [Column("channel_id")]
        public string? ChannelId { get; set; }
        [Column("inviter_id")]
        public string? InviterId { get; set; }
        [Column("target_user_id")]
        public string? TargetUserId { get; set; }
        [Column("target_user_type")]
        public int? TargetUserType { get; set; }
        [Column("vanity_url")]
        public bool? VanityUrl { get; set; }

        [ForeignKey("ChannelId")]
        [InverseProperty("Invites")]
        public virtual Channel? Channel { get; set; }
        [ForeignKey("GuildId")]
        [InverseProperty("Invites")]
        public virtual Guild? Guild { get; set; }
        [ForeignKey("InviterId")]
        [InverseProperty("InviteInviters")]
        public virtual User? Inviter { get; set; }
        [ForeignKey("TargetUserId")]
        [InverseProperty("InviteTargetUsers")]
        public virtual User? TargetUser { get; set; }
    }
}
