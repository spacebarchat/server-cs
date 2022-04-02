using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("sticker_packs")]
    public partial class StickerPack
    {
        public StickerPack()
        {
            Stickers = new HashSet<Sticker>();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("description")]
        public string? Description { get; set; }
        [Column("banner_asset_id")]
        public string? BannerAssetId { get; set; }
        [Column("cover_sticker_id")]
        public string? CoverStickerId { get; set; }
        [Column("coverStickerId")]
        public string? CoverStickerId1 { get; set; }

        [ForeignKey("CoverStickerId1")]
        [InverseProperty("StickerPacks")]
        public virtual Sticker? CoverStickerId1Navigation { get; set; }
        [InverseProperty("Pack")]
        public virtual ICollection<Sticker> Stickers { get; set; }
    }
}
