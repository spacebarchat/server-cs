namespace Fosscord.ConfigModel.Api;

public class ApiConfig
{
    public AssetCacheConfig AssetCache { get; set; } = new();
    public ApiDebugConfig Debug { get; set; } = new();
}