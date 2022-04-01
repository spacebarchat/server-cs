using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("applications")]
    public partial class Application
    {
        public Application()
        {
            Messages = new HashSet<Message>();
            Webhooks = new HashSet<Webhook>();
        }

        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; } = null!;
        [Column("icon", TypeName = "character varying")]
        public string? Icon { get; set; }
        [Column("description", TypeName = "character varying")]
        public string Description { get; set; } = null!;
        [Column("rpc_origins")]
        public string? RpcOrigins { get; set; }
        [Column("bot_public")]
        public bool BotPublic { get; set; }
        [Column("bot_require_code_grant")]
        public bool BotRequireCodeGrant { get; set; }
        [Column("terms_of_service_url", TypeName = "character varying")]
        public string? TermsOfServiceUrl { get; set; }
        [Column("privacy_policy_url", TypeName = "character varying")]
        public string? PrivacyPolicyUrl { get; set; }
        [Column("summary", TypeName = "character varying")]
        public string? Summary { get; set; }
        [Column("verify_key", TypeName = "character varying")]
        public string VerifyKey { get; set; } = null!;
        [Column("primary_sku_id", TypeName = "character varying")]
        public string? PrimarySkuId { get; set; }
        [Column("slug", TypeName = "character varying")]
        public string? Slug { get; set; }
        [Column("cover_image", TypeName = "character varying")]
        public string? CoverImage { get; set; }
        [Column("flags", TypeName = "character varying")]
        public string Flags { get; set; } = null!;
        [Column("owner_id", TypeName = "character varying")]
        public string? OwnerId { get; set; }
        [Column("team_id", TypeName = "character varying")]
        public string? TeamId { get; set; }
        [Column("guild_id", TypeName = "character varying")]
        public string? GuildId { get; set; }

        [ForeignKey("GuildId")]
        [InverseProperty("Applications")]
        public virtual Guild? Guild { get; set; }
        [ForeignKey("OwnerId")]
        [InverseProperty("Applications")]
        public virtual User? Owner { get; set; }
        [ForeignKey("TeamId")]
        [InverseProperty("Applications")]
        public virtual Team? Team { get; set; }
        [InverseProperty("Application")]
        public virtual ICollection<Message> Messages { get; set; }
        [InverseProperty("Application")]
        public virtual ICollection<Webhook> Webhooks { get; set; }
    }
}
