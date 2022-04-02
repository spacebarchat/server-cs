using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.DbModel.Scaffold
{
    [Table("messages")]
    [Index("AuthorId", Name = "IDX_05535bc695e9f7ee104616459d")]
    [Index("ChannelId", "Id", Name = "IDX_3ed7a60fb7dbe04e1ba9332a8b", IsUnique = true)]
    [Index("ChannelId", Name = "IDX_86b9109b155eb70c0a2ca3b4b6")]
    public partial class Message
    {
        public Message()
        {
            Attachments = new HashSet<Attachment>();
            InverseMessageReferenceNavigation = new HashSet<Message>();
            Channels = new HashSet<Channel>();
            Roles = new HashSet<Role>();
            Stickers = new HashSet<Sticker>();
            Users = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("channel_id")]
        public string? ChannelId { get; set; }
        [Column("guild_id")]
        public string? GuildId { get; set; }
        [Column("author_id")]
        public string? AuthorId { get; set; }
        [Column("member_id")]
        public int? MemberId { get; set; }
        [Column("webhook_id")]
        public string? WebhookId { get; set; }
        [Column("application_id")]
        public string? ApplicationId { get; set; }
        [Column("content")]
        public string? Content { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
        [Column("edited_timestamp")]
        public DateTime? EditedTimestamp { get; set; }
        [Column("tts")]
        public bool? Tts { get; set; }
        [Column("mention_everyone")]
        public bool? MentionEveryone { get; set; }
        [Column("embeds")]
        public string Embeds { get; set; } = null!;
        [Column("reactions")]
        public string Reactions { get; set; } = null!;
        [Column("nonce")]
        public string? Nonce { get; set; }
        [Column("pinned")]
        public bool? Pinned { get; set; }
        [Column("activity")]
        public string? Activity { get; set; }
        [Column("flags")]
        public string? Flags { get; set; }
        [Column("message_reference")]
        public string? MessageReference { get; set; }
        [Column("interaction")]
        public string? Interaction { get; set; }
        [Column("components")]
        public string? Components { get; set; }
        [Column("message_reference_id")]
        public string? MessageReferenceId { get; set; }
        [Column("type")]
        public int Type { get; set; }

        [ForeignKey("ApplicationId")]
        [InverseProperty("Messages")]
        public virtual Application? Application { get; set; }
        [ForeignKey("AuthorId")]
        [InverseProperty("Messages")]
        public virtual User? Author { get; set; }
        [ForeignKey("ChannelId")]
        [InverseProperty("Messages")]
        public virtual Channel? Channel { get; set; }
        [ForeignKey("GuildId")]
        [InverseProperty("Messages")]
        public virtual Guild? Guild { get; set; }
        [ForeignKey("MemberId")]
        [InverseProperty("Messages")]
        public virtual Member? Member { get; set; }
        [ForeignKey("MessageReferenceId")]
        [InverseProperty("InverseMessageReferenceNavigation")]
        public virtual Message? MessageReferenceNavigation { get; set; }
        [ForeignKey("WebhookId")]
        [InverseProperty("Messages")]
        public virtual Webhook? Webhook { get; set; }
        [InverseProperty("Message")]
        public virtual ICollection<Attachment> Attachments { get; set; }
        [InverseProperty("MessageReferenceNavigation")]
        public virtual ICollection<Message> InverseMessageReferenceNavigation { get; set; }

        [ForeignKey("MessagesId")]
        [InverseProperty("MessagesNavigation")]
        public virtual ICollection<Channel> Channels { get; set; }
        [ForeignKey("MessagesId")]
        [InverseProperty("Messages")]
        public virtual ICollection<Role> Roles { get; set; }
        [ForeignKey("MessagesId")]
        [InverseProperty("Messages")]
        public virtual ICollection<Sticker> Stickers { get; set; }
        [ForeignKey("MessagesId")]
        [InverseProperty("MessagesNavigation")]
        public virtual ICollection<User> Users { get; set; }
    }
}
