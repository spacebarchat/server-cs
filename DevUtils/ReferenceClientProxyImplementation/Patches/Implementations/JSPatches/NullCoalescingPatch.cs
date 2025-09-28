using System.Text;
using System.Text.RegularExpressions;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class NullCoalescingPatch : IPatch {
    public int GetOrder() => 1;

    public string GetName() => "Patch null-coalescing expressions in JS";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string relativePath, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        stringContent = NullCoalescingRegex().Replace(
            stringContent,
            m => $"{m.Groups[1].Value}?.{m.Groups[2].Value}"
        );
        stringContent = ParenNullCheckRegex().Replace(
            stringContent,
            m => $"{m.Groups[1].Value} == null"
        );

        return Encoding.UTF8.GetBytes(stringContent);
    }

    [GeneratedRegex(@"null == ([a-zA-Z0-9_]+?) \? undefined : \1\.([a-zA-Z0-9_]+?)", RegexOptions.Compiled)]
    private static partial Regex NullCoalescingRegex();

    [GeneratedRegex(@"\(([^()]+?)\) == null", RegexOptions.Compiled)]
    private static partial Regex ParenNullCheckRegex();
}