using Spacebar.DbModel.Entities;

namespace Spacebar.Util.Schemas;

public class GuildCreateRequestSchema : IRequestSchema
{
        public string Name;
        public string? Region;
        public string? Icon;
        public ChannelModifySchema[] Channels;
        public string? GuildTemplateCode;
        public string? SystemChannelId;
        public string? RulesChannelId;
        public User User { get; set; }
}