using ReferenceClientProxyImplementation.Configuration;
using Spacebar.Util;

namespace ReferenceClientProxyImplementation.Tasks.Startup;

public class BuildClientTask(ProxyConfiguration proxyConfig) : ITask {
    public int GetOrder() => 10;

    public string GetName() => "Build updated test client";

    public async Task Execute() {
        var hc = new HttpClient();
        if (proxyConfig.AssetCache.WipeOnStartup) {
            Directory.Delete(proxyConfig.AssetCache.DiskCachePath, true);
            Directory.CreateDirectory(proxyConfig.AssetCache.DiskCachePath);
        }

        if (!proxyConfig.TestClient.Enabled ||
            !proxyConfig.TestClient.UseLatest) {
            Console.WriteLine("[Client Updater] Test client is disabled or not set to use latest version, skipping!");
            return;
        }

        Console.WriteLine("[Client updater] Fetching client");
        var client = HtmlUtils.CleanupHtml(await hc.GetStringAsync("https://canary.discord.com/channels/@me"));
        Console.WriteLine("[Client updater] Building client...");
        var target = File.ReadAllText("Resources/Pages/index-template.html");
        var lines = client.Split("\n");
        target = target.Replace("<!--prefetch_script-->",
            string.Join("\n", lines.Where(x => x.Contains("link rel=\"prefetch\" as=\"script\""))));
        target = target.Replace("<!--client_script-->",
            string.Join("\n", lines.Where(x => x.Contains("<script src="))));
        target = target.Replace("<!--client_css-->",
            string.Join("\n", lines.Where(x => x.Contains("link rel=\"stylesheet\""))));
        target = target.Replace("integrity", "hashes");
        File.WriteAllText("Resources/Pages/index-updated.html", target);
        Console.WriteLine("[Client updater] Finished building client!");
    }
}