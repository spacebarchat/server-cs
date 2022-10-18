using System.Collections;

namespace Fosscord.Static.Extensions;

public static class BitArrayExtensions
{
    public static bool HasFlag(this BitArray bitArray, int index)
    {
        return bitArray.Length >= index && bitArray[index];
    }
}