using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Keyless]
    [Table("typeorm_metadata")]
    public partial class TypeormMetadatum
    {
        [Column("type", TypeName = "character varying")]
        public string Type { get; set; } = null!;
        [Column("database", TypeName = "character varying")]
        public string? Database { get; set; }
        [Column("schema", TypeName = "character varying")]
        public string? Schema { get; set; }
        [Column("table", TypeName = "character varying")]
        public string? Table { get; set; }
        [Column("name", TypeName = "character varying")]
        public string? Name { get; set; }
        [Column("value")]
        public string? Value { get; set; }
    }
}
