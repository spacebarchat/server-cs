using System.Text;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public class BooleanPatch : IPatch {
    public int GetOrder() => 0;

    public string GetName() => "Use real booleans in JS files";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        stringContent = stringContent
            .Replace("return!", "return !")
            .Replace("!0", "true")
            .Replace("!1", "false");

        return Encoding.UTF8.GetBytes(stringContent);
    }
}