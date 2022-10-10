using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosscord.DbModel.Scaffold
{
    [Table("query-result-cache")]
    public partial class QueryResultCache
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("identifier", TypeName = "character varying")]
        public string? Identifier { get; set; }
        [Column("time")]
        public long Time { get; set; }
        [Column("duration")]
        public int Duration { get; set; }
        [Column("query")]
        public string Query { get; set; } = null!;
        [Column("result")]
        public string Result { get; set; } = null!;
    }
}
