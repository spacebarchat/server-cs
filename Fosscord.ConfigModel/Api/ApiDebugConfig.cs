namespace Fosscord.ConfigModel.Api;

public class ApiDebugConfig
{
    public bool ReformatAssets { get; } = false;
    public string FormattedAssetPath { get; } = "cache_formatted";
    public bool OpenFormattedDirAfterReformat { get; } = false;
    public (string Command, string Args) OpenFormattedDirCommand { get; } = ("code-insiders", "$dir");
}