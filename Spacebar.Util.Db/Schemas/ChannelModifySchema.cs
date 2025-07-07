namespace Spacebar.Util.Schemas;

public class ChannelModifySchema {
    public int? Bitrate;
    public int? DefaultAutoArchiveDuration;
    public int? DefaultThreadRateLimitPerUser;
    public int? Flags;
    public string? Icon;
    public string? Id;
    public string? Name;
    public bool? Nsfw;
    public string? ParentId;
    public PermissionOverwrite[]? PermissionOverwrites;
    public int? Position;
    public int? RateLimitPerUser;
    public string? RtcRegion;
    public string? Topic;
    public ChannelType? Type;
    public int? UserLimit;
}

public class PermissionOverwrite {
    public string Allow;
    public string Deny;
    public string Id;
    public ChannelPermissionOverwriteType Type;
}

public enum ChannelPermissionOverwriteType {
    Role = 0,
    Member = 1,
    Group = 2
}