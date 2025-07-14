using System.Text;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public class KnownConstantsPatch : IPatch {
    public int GetOrder() => 1;

    public string GetName() => "Use named constants";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        return Encoding.UTF8.GetBytes(stringContent);
    }
}