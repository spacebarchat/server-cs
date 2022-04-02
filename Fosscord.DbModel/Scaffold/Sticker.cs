using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("stickers")]
    public partial class Sticker
    {
        public Sticker()
        {
            StickerPacks = new HashSet<StickerPack>();
            Messages = new HashSet<Message>();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("description")]
        public string? Description { get; set; }
        [Column("tags")]
        public string? Tags { get; set; }
        [Column("pack_id")]
        public string? PackId { get; set; }
        [Column("guild_id")]
        public string? GuildId { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("format_type")]
        public int FormatType { get; set; }
        [Column("available")]
        public bool? Available { get; set; }
        [Column("user_id")]
        public string? UserId { get; set; }

        [ForeignKey("GuildId")]
        [InverseProperty("Stickers")]
        public virtual Guild? Guild { get; set; }
        [ForeignKey("PackId")]
        [InverseProperty("Stickers")]
        public virtual StickerPack? Pack { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Stickers")]
        public virtual User? User { get; set; }
        [InverseProperty("CoverStickerId1Navigation")]
        public virtual ICollection<StickerPack> StickerPacks { get; set; }

        [ForeignKey("StickersId")]
        [InverseProperty("Stickers")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
