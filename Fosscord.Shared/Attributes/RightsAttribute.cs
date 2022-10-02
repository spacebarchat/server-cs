using System.Collections;
using Fosscord.Shared.Enums;
using Fosscord.Util.Generic.Extensions;

namespace Fosscord.Shared.Attributes;

public class RequireRightsAttribute : Attribute
{
    public int? right = null;
    public int[]? allRights = null;
    public int[]? anyRights = null;

    public RequireRightsAttribute(Rights? right = null, Rights[]? allRights = null, Rights[]? anyRights = null)
    {
        this.right = right;
        this.allRights = allRights;
        this.anyRights = anyRights;
    }

    public bool HasRights(BitArray rights)
    {
        if (rights.HasFlag(Rights.OPERATOR)) return true;
        if (anyRights is not null && !anyRights.Any(x => rights.HasFlag(x))) return false;
        if (allRights is not null && !allRights.All(x => userRights.HasFlag(x))) return false;
        if (right is not null && !userRights.HasFlag(right)) return false;
        return true;
    }

    private bool hasFlag(BitArray array, int position)
    {
        return array.Length >= position && array[position] == true;
    }
}