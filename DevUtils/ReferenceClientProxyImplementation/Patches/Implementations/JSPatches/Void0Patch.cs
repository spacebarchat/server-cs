using System.Text;
using System.Text.RegularExpressions;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class Void0Patch : IPatch {
    public int GetOrder() => 0;

    public string GetName() => "Use literal undefined instead of void 0";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        stringContent = stringContent
            .Replace("void 0", "undefined");
        stringContent = VoidFunctionRegex().Replace(
            stringContent,
            m => $"{m.Groups[1].Value}("
        );

        return Encoding.UTF8.GetBytes(stringContent);
    }
    
    [GeneratedRegex(@"\(0, ([a-zA-Z0-9_.$]+?)\)\(", RegexOptions.Compiled)]
    private static partial Regex VoidFunctionRegex();
}