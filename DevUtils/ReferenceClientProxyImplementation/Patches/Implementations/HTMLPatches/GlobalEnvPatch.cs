using System.Text;
using System.Text.RegularExpressions;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Patches.Implementations.HTMLPatches;

public class GlobalEnvPatch(ProxyConfiguration config) : IPatch {
    public int GetOrder() => 1;

    public string GetName() => "Patch GLOBAL_ENV";
    public bool Applies(string relativeName, byte[] content) => relativeName is "app.html" or "developers.html" or "popout.html";

    public async Task<byte[]> Execute(string _, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        foreach(var (key, value) in config.TestClient.GlobalEnv) {
            stringContent = new Regex($"{key}: \".*?\"").Replace(stringContent, $"{key}: \"{value}\"");
        }
        
        return Encoding.UTF8.GetBytes(stringContent);
    }
}