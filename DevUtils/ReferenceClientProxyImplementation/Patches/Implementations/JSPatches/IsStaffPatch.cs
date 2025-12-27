using System.Text;
using System.Text.RegularExpressions;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class IsStaffPatch : IPatch {
    public int GetOrder() => 2;

    public string GetName() => "Patch isStaff/isStaffPersonal in JS";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string relativePath, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        stringContent = IsNullableStaffRegex().Replace(
            stringContent,
            m => $"{m.Groups[1].Value}!!{m.Groups[2].Value}"
        );
        
        stringContent = IsStaffRegex().Replace(
            stringContent,
            m => $"{m.Groups[1].Value}true"
        );

        return Encoding.UTF8.GetBytes(stringContent);
    }

    [GeneratedRegex(@"(\W)(\w|this|\w\.user)\.isStaff(Personal)?\(\)", RegexOptions.Compiled)]
    private static partial Regex IsStaffRegex();
    
    [GeneratedRegex(@"(\W)(\w|this|\w\.user)\?\.isStaff(Personal)?\(\)", RegexOptions.Compiled)]
    private static partial Regex IsNullableStaffRegex();
}