using ArcaneLibs.Extensions.Streams;
using ReferenceClientProxyImplementation.Configuration;
using ReferenceClientProxyImplementation.Patches.Implementations;

namespace ReferenceClientProxyImplementation.Services;

public class ClientStoreService(ProxyConfiguration config, PatchSet patches) {
    private static readonly HttpClient HttpClient = new();
    
    public async Task<Stream> GetPatchedClientAsset(string relativePath) {
        if (relativePath.StartsWith("/")) {
            relativePath = relativePath[1..];
        }
        
        var path = Path.Combine(config.TestClient.RevisionPath, "patched", relativePath);

        if (File.Exists(path))
            return File.OpenRead(path);
        
        var srcAsset = (await GetOrDownloadRawAsset(relativePath)).ReadToEnd().ToArray();
        var result = await patches.ApplyPatches(relativePath, srcAsset);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        if (!result.SequenceEqual(srcAsset)) {
            await File.WriteAllBytesAsync(path, result);
            return File.OpenRead(path);
        }
        
        Console.WriteLine($"No patches applied for {relativePath}, returning original asset.");
        return new MemoryStream(srcAsset);
    }

    public async Task<Stream> GetOrDownloadRawAsset(string relativePath) {
        relativePath = relativePath.TrimStart('/');
        var assetPath = Path.Combine(config.TestClient.RevisionPath, "src", relativePath);
        if (File.Exists(assetPath)) {
            Console.WriteLine($"Asset {relativePath} already exists at {assetPath}, returning existing file.");
            return File.OpenRead(assetPath);
        }
        
        var url = $"{config.TestClient.RevisionBaseUrl}/{relativePath}";
        Console.WriteLine($"Downloading asset {relativePath} from {url}");
        var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode) {
            Console.WriteLine($"Failed to download asset {relativePath} from {url}, status code: {response.StatusCode}");
            throw new FileNotFoundException($"Asset not found: {relativePath}");
        }
        var contentStream = await response.Content.ReadAsStreamAsync();
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath)!);
        await using var fileStream = File.Create(assetPath);
        await contentStream.CopyToAsync(fileStream);
        fileStream.Close();
        contentStream.Close();
        Console.WriteLine($"Downloaded asset {relativePath} to {assetPath}");

        return File.OpenRead(assetPath);
    }

    public bool HasRawAsset(string relativePath) {
        relativePath = relativePath.TrimStart('/');
        var assetPath = Path.Combine(config.TestClient.RevisionPath, "src", relativePath);
        return File.Exists(assetPath);
    }
}