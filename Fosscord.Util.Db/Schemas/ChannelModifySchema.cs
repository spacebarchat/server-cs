namespace Fosscord.Util.Schemas;

public class ChannelModifySchema
{
    public string? Name;
    public ChannelType? Type;
    public string? Topic;
    public string? Icon;
    public int? Bitrate;
    public int? UserLimit;
    public int? RateLimitPerUser;
    public int? Position;
    public string? ParentId;
    public string? Id;
    public bool? Nsfw;
    public string? RtcRegion;
    public int? DefaultAutoArchiveDuration;
    public int? Flags;
    public int? DefaultThreadRateLimitPerUser;
    public PermissionOverwrite[]? PermissionOverwrites;
}

public class PermissionOverwrite
{
    public string Id;
    public ChannelPermissionOverwriteType Type;
    public string Allow;
    public string Deny;
}

public enum ChannelPermissionOverwriteType
{
    Role = 0,
    Member = 1,
    Group = 2
}