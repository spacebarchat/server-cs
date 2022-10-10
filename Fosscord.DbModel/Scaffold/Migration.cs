using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosscord.DbModel.Scaffold
{
    [Table("migrations")]
    public partial class Migration
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("timestamp")]
        public long Timestamp { get; set; }
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; } = null!;
    }
}
