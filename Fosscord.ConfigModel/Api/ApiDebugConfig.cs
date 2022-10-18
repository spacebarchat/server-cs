namespace Fosscord.ConfigModel.Api;

public class ApiDebugConfig
{
    public bool ReformatAssets { get; set; } = false;
    public string FormattedAssetPath { get; set; } = "cache_formatted";
    public bool OpenFormattedDirAfterReformat { get; set; } = false;
    public (string Command, string Args) OpenFormattedDirCommand { get; set; } = ("code-insiders", "$dir");
}