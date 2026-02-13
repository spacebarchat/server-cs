using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ArcaneLibs.Extensions;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Tasks.Startup;

public partial class InitClientStoreService(ProxyConfiguration proxyConfig) : ITask {
    public int GetOrder() => 0;

    public string GetName() => "Get client revision";

    public async Task Execute() {
        switch (proxyConfig.TestClient.Revision) {
            case "canary":
                proxyConfig.TestClient.RevisionBaseUrl = "https://canary.discord.com";
                proxyConfig.TestClient.RevisionPath = await GetRevisionPathFromUrl("canary", "https://canary.discord.com/app");
                break;
            case "ptb":
                proxyConfig.TestClient.RevisionBaseUrl = "https://ptb.discord.com";
                proxyConfig.TestClient.RevisionPath = await GetRevisionPathFromUrl("ptb", "https://ptb.discord.com/app");
                break;
            case "stable":
                proxyConfig.TestClient.RevisionBaseUrl = "https://discord.com";
                proxyConfig.TestClient.RevisionPath = await GetRevisionPathFromUrl("stable", "https://discord.com/app");
                break;
            default:
                if (proxyConfig.TestClient.RevisionPath == null) {
                    throw new Exception("Test client revision path is not set!");
                }

                break;
        }
    }

    private async Task<string> GetRevisionPathFromUrl(string rev, string url) {
        using var hc = new HttpClient();
        using var response = await hc.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var normalisedContent = StripNonces(content);
        var hash = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(normalisedContent));
        var knownHashes = await GetKnownRevisionHashes("src/app.html");
        var currentRevisionFilePath = Path.Combine(proxyConfig.AssetCache.DiskCacheBaseDirectory, "currentRevision");
        var previousRevision = Path.Exists(currentRevisionFilePath) ? await File.ReadAllTextAsync(currentRevisionFilePath) : "";
        var revisionName = rev;

        if (knownHashes.Any(x => x.Value.SequenceEqual(hash))) {
            Console.WriteLine($"[InitClientStoreTask] Found known revision '{rev}' with hash {hash.AsHexString().Replace(" ", "")}!");
            revisionName = knownHashes.First(x => x.Value.SequenceEqual(hash)).Key;
        }
        else {
            Console.WriteLine($"[InitClientStoreTask] No known revision found for hash {hash.AsHexString().Replace(" ", "")}, creating new revision directory!");
            if (response.Headers.Contains("X-Build-Id")) {
                revisionName = "buildId_" + response.Headers.GetValues("X-Build-Id").FirstOrDefault();
                Console.WriteLine("[InitClientStoreTask] Using build ID from X-Build-Id header: " + revisionName);
            }
        }

        var revisionPath = Path.Combine(proxyConfig.AssetCache.DiskCacheBaseDirectory, revisionName);
        Console.WriteLine($"[InitClientStoreTask] Saving revision '{revisionName}' to {revisionPath}...");
        PrepareRevisionDirectory(revisionPath);
        await File.WriteAllTextAsync(Path.Combine(revisionPath, "src", "app.html"), content);
        await File.WriteAllTextAsync(Path.Combine(proxyConfig.AssetCache.DiskCacheBaseDirectory, "currentRevision"), revisionName);

        //also download dev page
        using var devResponse = await hc.GetAsync(url.Replace("/app", "/developers/applications"));
        var devContent = await devResponse.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(Path.Combine(revisionPath, "src", "developers.html"), devContent);
        
        //...and popout
        using var popoutResponse = await hc.GetAsync(url.Replace("/app", "/popout"));
        var popoutContent = await popoutResponse.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(Path.Combine(revisionPath, "src", "popout.html"), popoutContent);
        
        if (proxyConfig.AssetCache.DitchPatchedOnStartup) {
            Directory.Delete(Path.Combine(revisionPath, "patched"), true);
            Directory.CreateDirectory(Path.Combine(revisionPath, "patched"));
        }

        if (previousRevision != revisionName || true) {
            foreach (var argv in proxyConfig.AssetCache.ExecOnRevisionChange) {
                try {
                    var psi = new ProcessStartInfo(argv[0], argv[1..].Select(a => a.Replace("{revisionPath}", revisionPath))) {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using var process = Process.Start(psi);
                    if (process != null) {
                        _ = process.StandardOutput.ReadToEndAsync();
                        _ = process.StandardError.ReadToEndAsync();
                        Console.WriteLine($"[InitClientStoreTask] Executing post-revision change command: {argv[0]} {string.Join(" ", argv[1..])}");
                    }
                    else {
                        Console.WriteLine($"[InitClientStoreTask] Failed to start post-revision change command: {argv[0]} {string.Join(" ", argv[1..])}");
                    }
                }
                catch (Exception e) {
                    Console.WriteLine($"[InitClientStoreTask] Failed to start post-revision change command: {argv[0]} {string.Join(" ", argv[1..])}\n{e}");
                }
            }
        }

        return revisionPath;
    }

    private static void PrepareRevisionDirectory(string revisionPath, bool dropPatched = false) {
        Directory.CreateDirectory(revisionPath);
        Directory.CreateDirectory(Path.Combine(revisionPath, "src"));
        Directory.CreateDirectory(Path.Combine(revisionPath, "formatted"));
        Directory.CreateDirectory(Path.Combine(revisionPath, "patched"));
    }

    private async Task<Dictionary<string, byte[]>> GetKnownRevisionHashes(string file) {
        if (!Directory.Exists(proxyConfig.AssetCache.DiskCacheBaseDirectory))
            Directory.CreateDirectory(proxyConfig.AssetCache.DiskCacheBaseDirectory);

        var revisionHashTasks = Directory
            .GetDirectories(proxyConfig.AssetCache.DiskCacheBaseDirectory)
            .Select(dir => GetKnownRevisionHash(dir, file));

        var revisionHashes = await Task.WhenAll(revisionHashTasks);
        return revisionHashes
            .OfType<(string RevisionId, byte[] Hash)>()
            .ToDictionary(
                x => x.RevisionId,
                x => x.Hash
            );
    }

    private async Task<(string RevisionId, byte[] Hash)?> GetKnownRevisionHash(string dir, string file) {
        var hashFile = Path.Combine(dir, file);
        if (File.Exists(hashFile)) {
            var content = StripNonces(await File.ReadAllTextAsync(hashFile));
            var hash = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(content));
            var result = (new DirectoryInfo(dir).Name, hash);
            return result;
        }

        Console.WriteLine($"[InitClientStoreTask] '{file}' not found in client revision directory '{dir}'!");
        return null;
    }

    private static string StripNonces(string content) =>
        // most specific first
        HtmlScriptIntegrityRegex().Replace(
            HtmlScriptNonceRegex().Replace(
                JsElementNonceRegex().Replace(
                    CFParamsRegex().Replace(
                        content,
                        ""
                    ),
                    ""
                ),
                ""),
            ""
        );

    [GeneratedRegex("nonce=\"[a-zA-Z0-9+/=]+\"")]
    private static partial Regex HtmlScriptNonceRegex();

    [GeneratedRegex(@"\sintegrity=""[a-zA-Z0-9+/=\-\s]+""")]
    private static partial Regex HtmlScriptIntegrityRegex();

    [GeneratedRegex("\\w.nonce='[a-zA-Z0-9+/=]+';")]
    private static partial Regex JsElementNonceRegex();

    [GeneratedRegex(
        @"var\s+\w+\s*=\s*b\.createElement\('script'\);\s*\w+\.nonce='[a-zA-Z0-9+/=]+'\s*;\s*\w+\.innerHTML=""window\.__CF\$cv\$\w+=\{r:'[a-zA-Z0-9+/=]+',t:'[a-zA-Z0-9+/=]+'\};var\s+\w+=document\.createElement\('script'\);\s*\w+\.nonce='[a-zA-Z0-9+/=]+'\s*;\s*\w+\.src='/cdn-cgi/challenge-platform/scripts/jsd/main.js';document\.getElementsByTagName\('head'\)\[0\]\.appendChild\(\w+\);")]
    public static partial Regex CFParamsRegex();
}