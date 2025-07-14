using ArcaneLibs.Extensions;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Tasks.Startup;

public partial class SanitiseConfigPathTask(ProxyConfiguration proxyConfig) : ITask {
    public int GetOrder() => int.MinValue;

    public string GetName() => "Sanitise config path";

    public async Task Execute() {
         // proxyConfig.AssetCache.DiskCacheBaseDirectory = Path.GetFullPath(proxyConfig.AssetCache.DiskCacheBaseDirectory);
         Console.WriteLine(proxyConfig.ToJson());
    }
}