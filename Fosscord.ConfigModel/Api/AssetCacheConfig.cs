namespace Fosscord.ConfigModel.Api;

public class AssetCacheConfig
{
    public bool MemoryCache { get; } = true;
    public bool DiskCache { get; } = true;
    public string DiskCachePath { get; } = "cache";
    public bool WipeOnStartup { get; } = false;
}