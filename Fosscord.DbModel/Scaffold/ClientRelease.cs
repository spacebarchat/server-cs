using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosscord.DbModel.Scaffold
{
    [Table("client_release")]
    public partial class ClientRelease
    {
        [Key]
        [Column("id", TypeName = "character varying")]
        public string Id { get; set; } = null!;
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; } = null!;
        [Column("pub_date", TypeName = "character varying")]
        public string PubDate { get; set; } = null!;
        [Column("url", TypeName = "character varying")]
        public string Url { get; set; } = null!;
        [Column("deb_url", TypeName = "character varying")]
        public string DebUrl { get; set; } = null!;
        [Column("osx_url", TypeName = "character varying")]
        public string OsxUrl { get; set; } = null!;
        [Column("win_url", TypeName = "character varying")]
        public string WinUrl { get; set; } = null!;
        [Column("notes", TypeName = "character varying")]
        public string? Notes { get; set; }
    }
}
