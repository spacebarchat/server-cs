using Spacebar.DbModel.Entities;

namespace Spacebar.Util.Schemas;

public class GuildCreateRequestSchema : IRequestSchema {
    public ChannelModifySchema[] Channels;
    public string? GuildTemplateCode;
    public string? Icon;
    public string Name;
    public string? Region;
    public string? RulesChannelId;
    public string? SystemChannelId;
    public User User { get; set; }
}