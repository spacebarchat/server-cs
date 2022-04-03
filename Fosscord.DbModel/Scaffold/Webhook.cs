﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("webhooks")]
    public partial class Webhook
    {
        public Webhook()
        {
            Messages = new HashSet<Message>();
        }

        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("name", TypeName = "character varying")]
        public string? Name { get; set; }
        [Column("avatar", TypeName = "character varying")]
        public string? Avatar { get; set; }
        [Column("token", TypeName = "character varying")]
        public string? Token { get; set; }
        [Column("guild_id", TypeName = "character varying")]
        public string? GuildId { get; set; }
        [Column("channel_id", TypeName = "character varying")]
        public string? ChannelId { get; set; }
        [Column("application_id", TypeName = "character varying")]
        public string? ApplicationId { get; set; }
        [Column("user_id", TypeName = "character varying")]
        public string? UserId { get; set; }
        [Column("source_guild_id", TypeName = "character varying")]
        public string? SourceGuildId { get; set; }
        [Column("type")]
        public int Type { get; set; }

        [ForeignKey("ApplicationId")]
        [InverseProperty("Webhooks")]
        public virtual Application? Application { get; set; }
        [ForeignKey("ChannelId")]
        [InverseProperty("Webhooks")]
        public virtual Channel? Channel { get; set; }
        [ForeignKey("GuildId")]
        [InverseProperty("WebhookGuilds")]
        public virtual Guild? Guild { get; set; }
        [ForeignKey("SourceGuildId")]
        [InverseProperty("WebhookSourceGuilds")]
        public virtual Guild? SourceGuild { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Webhooks")]
        public virtual User? User { get; set; }
        [InverseProperty("Webhook")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
