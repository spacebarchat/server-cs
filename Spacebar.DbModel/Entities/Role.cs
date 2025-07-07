﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spacebar.DbModel.Entities;

[Table("roles")]
public class Role {
    [Key]
    [Column("id", TypeName = "character varying")]
    public string Id { get; set; } = null!;

    [Column("guild_id", TypeName = "character varying")]
    public string? GuildId { get; set; }

    [Column("color")]
    public int Color { get; set; }

    [Column("hoist")]
    public bool Hoist { get; set; }

    [Column("managed")]
    public bool Managed { get; set; }

    [Column("mentionable")]
    public bool Mentionable { get; set; }

    [Column("name", TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column("permissions", TypeName = "character varying")]
    public string Permissions { get; set; } = null!;

    [Column("position")]
    public int Position { get; set; }

    [Column("icon", TypeName = "character varying")]
    public string? Icon { get; set; }

    [Column("unicode_emoji", TypeName = "character varying")]
    public string? UnicodeEmoji { get; set; }

    [Column("tags")]
    public string? Tags { get; set; }

    [ForeignKey("GuildId")]
    [InverseProperty("Roles")]
    public virtual Guild? Guild { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<Member> Indices { get; set; } = new HashSet<Member>();

    [ForeignKey("RolesId")]
    [InverseProperty("Roles")]
    public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
}