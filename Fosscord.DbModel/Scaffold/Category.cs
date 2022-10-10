using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosscord.DbModel.Scaffold
{
    [Table("categories")]
    public partial class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name", TypeName = "character varying")]
        public string? Name { get; set; }
        [Column("localizations")]
        public string Localizations { get; set; } = null!;
        [Column("is_primary")]
        public bool? IsPrimary { get; set; }
    }
}
