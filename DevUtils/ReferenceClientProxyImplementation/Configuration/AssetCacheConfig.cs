namespace ReferenceClientProxyImplementation.Configuration;

public class AssetCacheConfig {
    public bool MemoryCache { get; set; } = true;
    public bool DiskCache { get; set; } = true;
    public string DiskCachePath { get; set; } = "cache";
    public bool WipeOnStartup { get; set; } = false;
    public string DiskCacheBaseDirectory { get; set; } = "./clientRepository";
    public List<List<string>> ExecOnRevisionChange { get; set; } = [];
    public string BiomePath { get; set; } = "biome";
    public string PrettierPath { get; set; } = "prettier";

    public string NodePath { get; set; } = "node";
}