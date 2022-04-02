using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Keyless]
    [Table("typeorm_metadata")]
    public partial class TypeormMetadatum
    {
        [Column("type")]
        public string Type { get; set; } = null!;
        [Column("database")]
        public string? Database { get; set; }
        [Column("schema")]
        public string? Schema { get; set; }
        [Column("table")]
        public string? Table { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("value")]
        public string? Value { get; set; }
    }
}
