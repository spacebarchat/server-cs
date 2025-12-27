using System.Text;
using System.Text.RegularExpressions;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class ExpandUnicodeEscapesPatch : IPatch {
    public int GetOrder() => 0;

    public string GetName() => @"JS: expand \x?? to \u00??";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);
        
        stringContent = XToURegex().Replace(
            stringContent,
            m => $"\\u00{m.Groups[1].Value}"
        );

        return Encoding.UTF8.GetBytes(stringContent);
    }
    
    [GeneratedRegex(@"\\x([0-9A-Fa-f]{2})", RegexOptions.Compiled)]
    private static partial Regex XToURegex();
    
}