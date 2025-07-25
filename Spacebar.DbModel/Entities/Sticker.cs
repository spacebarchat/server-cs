﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spacebar.DbModel.Entities;

[Table("stickers")]
public class Sticker {
    [Key]
    [Column("id", TypeName = "character varying")]
    public string Id { get; set; } = null!;

    [Column("name", TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column("description", TypeName = "character varying")]
    public string? Description { get; set; }

    [Column("available")]
    public bool? Available { get; set; }

    [Column("tags", TypeName = "character varying")]
    public string? Tags { get; set; }

    [Column("pack_id", TypeName = "character varying")]
    public string? PackId { get; set; }

    [Column("guild_id", TypeName = "character varying")]
    public string? GuildId { get; set; }

    [Column("user_id", TypeName = "character varying")]
    public string? UserId { get; set; }

    [Column("type")]
    public int Type { get; set; }

    [Column("format_type")]
    public int FormatType { get; set; }

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
    public virtual ICollection<StickerPack> StickerPacks { get; set; } = new HashSet<StickerPack>();

    [ForeignKey("StickersId")]
    [InverseProperty("Stickers")]
    public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
}