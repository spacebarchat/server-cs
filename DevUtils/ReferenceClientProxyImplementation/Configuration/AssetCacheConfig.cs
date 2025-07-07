namespace ReferenceClientProxyImplementation.Configuration;

public class AssetCacheConfig {
    public bool MemoryCache { get; set; } = true;
    public bool DiskCache { get; set; } = true;
    public string DiskCachePath { get; set; } = "cache";
    public string DiskCacheBaseDirectory { get; set; } = "./clientRepository";
    public bool WipeOnStartup { get; set; } = false;
}