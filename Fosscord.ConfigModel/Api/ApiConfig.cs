namespace Fosscord.ConfigModel.Api;

public class ApiConfig
{
    public AssetCacheConfig AssetCache { get; } = new();
    public ApiDebugConfig Debug { get; } = new();
}