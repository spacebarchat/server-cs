using System.Collections;
using Fosscord.Shared.Enums;
using Fosscord.Util.Generic.Extensions;

namespace Fosscord.Shared.Attributes;

public class RequireRightsAttribute : Attribute
{
    public int right = -1;
    public int[]? allRights = null;
    public int[]? anyRights = null;

    /// <summary>
    /// Requires rights on a route
    /// </summary>
    /// <param name="right">Require single right</param>
    /// <param name="allRights">Require all of these rights</param>
    /// <param name="anyRights">Require any of these rights</param>
    public RequireRightsAttribute(int right = -1, int[] allRights = null, int[] anyRights = null)
    {
        this.right = right;
        this.allRights = allRights;
        this.anyRights = anyRights;
    }

    /// <summary>
    /// Verify wether a bitarray matches the specified rights
    /// </summary>
    /// <param name="rights">BitArray containing the user's rights</param>
    /// <returns>Whether the user has the required rights specified by the attribute</returns>
    public bool HasRights(BitArray rights)
    {
        //gConsole.WriteLine($"Checking rights: {right}, {allRights}, {anyRights}");
        if (rights.HasFlag(Rights.OPERATOR)) return true;
        if (anyRights is not null && !anyRights.Any(rights.HasFlag)) return false;
        if (allRights is not null && !allRights.All(rights.HasFlag)) return false;
        if (right is not -1 && !rights.HasFlag(right)) return false;
        return true;
    }
}