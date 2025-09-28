using System.Text;
using System.Text.RegularExpressions;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class WhileTruePatch : IPatch {
    public int GetOrder() => 1;

    public string GetName() => "Patch while(true) expressions in JS";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string relativePath, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        stringContent = stringContent.Replace("for (;;)", "while (true)");

        return Encoding.UTF8.GetBytes(stringContent);
    }
}