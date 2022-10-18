using System.Diagnostics;
using Fosscord.API.Controllers;
using Fosscord.ConfigModel;

namespace Fosscord.API.Tasks.Startup;

public class PatchClientAssetsTask : ITask
{
    public string GetName()
    {
        return "Patch client assets";
    }

    public void Execute()
    {
        foreach (var file in Directory.GetFiles(Config.Instance.Api.AssetCache.DiskCachePath).Where(x => x.EndsWith(".js")))
        {
            var start = DateTime.Now;
            if(Config.Instance.Logging.LogClientPatching) Console.Write($"[Client Patcher] Patching file {file}...");
            var contents = File.ReadAllText(file);
            contents = AssetsController.PatchClient(contents);
            File.WriteAllText(file, contents);
            if(Config.Instance.Logging.LogClientPatching) Console.WriteLine($" Done in {DateTime.Now - start}!");
        }

        if (Config.Instance.Api.Debug.ReformatAssets)
        {
            Console.WriteLine("[Client Patcher] Reformatting assets...");
            foreach (var file in Directory.GetFiles(Config.Instance.Api.AssetCache.DiskCachePath))
            {
                var target = file.Replace(Config.Instance.Api.AssetCache.DiskCachePath, Config.Instance.Api.Debug.FormattedAssetPath);
                if(!File.Exists(target))
                    File.Copy(file, target, false);
            }

            Process.Start("npx", "prettier -w " + Config.Instance.Api.Debug.FormattedAssetPath).WaitForExit();
            Console.WriteLine("[Client Patcher] Done!");
            if (Config.Instance.Api.Debug.OpenFormattedDirAfterReformat)
                Process.Start(Config.Instance.Api.Debug.OpenFormattedDirCommand.Command, Config.Instance.Api.Debug.OpenFormattedDirCommand.Args.Replace("$dir", Config.Instance.Api.Debug.FormattedAssetPath));
        }
    }
}