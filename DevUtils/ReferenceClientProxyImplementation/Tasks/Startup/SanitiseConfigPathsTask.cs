using System.Text;
using System.Text.RegularExpressions;
using ArcaneLibs;
using ArcaneLibs.Extensions;
using ReferenceClientProxyImplementation.Configuration;
using Spacebar.Util;

namespace ReferenceClientProxyImplementation.Tasks.Startup;

public partial class SanitiseConfigPathTask(ProxyConfiguration proxyConfig) : ITask {
    public int GetOrder() => int.MinValue;

    public string GetName() => "Sanitise config path";

    public async Task Execute() {
         // proxyConfig.AssetCache.DiskCacheBaseDirectory = Path.GetFullPath(proxyConfig.AssetCache.DiskCacheBaseDirectory);
    }
}