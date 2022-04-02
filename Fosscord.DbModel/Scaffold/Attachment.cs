using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("attachments")]
    public partial class Attachment
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("filename")]
        public string Filename { get; set; } = null!;
        [Column("size")]
        public int Size { get; set; }
        [Column("url")]
        public string Url { get; set; } = null!;
        [Column("proxy_url")]
        public string ProxyUrl { get; set; } = null!;
        [Column("height")]
        public int? Height { get; set; }
        [Column("width")]
        public int? Width { get; set; }
        [Column("content_type")]
        public string? ContentType { get; set; }
        [Column("message_id")]
        public string? MessageId { get; set; }

        [ForeignKey("MessageId")]
        [InverseProperty("Attachments")]
        public virtual Message? Message { get; set; }
    }
}
