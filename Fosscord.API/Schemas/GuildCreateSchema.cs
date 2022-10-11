namespace Fosscord.API.Schemas;

public class GuildCreateSchema
{
        public string Name;
        public string? Region;
        public string? Icon;
        public ChannelModifySchema[] Channels;
        public string? GuildTemplateCode;
        public string? SystemChannelId;
        public string? RulesChannelId;
}