using Fosscord.Shared.Enums;

namespace Fosscord.Shared.Attributes;

public class RequireRightsAttribute : Attribute
{
    public Rights? right = null;
    public Rights[]? allRights = null;
    public Rights[]? anyRights = null;

    public RequireRightsAttribute(Rights? right = null, Rights[]? allRights = null, Rights[]? anyRights = null)
    {
        this.right = right;
        this.allRights = allRights;
        this.anyRights = anyRights;
    }

    public bool HasRights(Rights userRights)
    {
        if (userRights.HasFlag(Rights.OPERATOR)) return true;
        if (anyRights is not null && !anyRights.Any(x => userRights.HasFlag(x))) return false;
        if (allRights is not null && !allRights.All(x => userRights.HasFlag(x))) return false;
        if (right is not null && !userRights.HasFlag(right)) return false;
        return true;
    }

    public bool HasRights(string rights)
    {
        Rights.TryParse(rights, out Rights _rights);
        return HasRights(_rights);
    }
}