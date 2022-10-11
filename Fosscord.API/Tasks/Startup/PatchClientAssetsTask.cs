using System.Diagnostics;
using System.Net;
using Fosscord.API.Controllers;
using Fosscord.API.Utilities;
using Fosscord.DbModel;

namespace Fosscord.API.Tasks.Startup;

public class PatchClientAssetsTask : ITask
{
    public string GetName()
    {
        return "Patch client assets";
    }

    public void Execute()
    {
        foreach (var file in Directory.GetFiles(Static.Config.Api.AssetCache.DiskCachePath).Where(x => x.EndsWith(".js")))
        {
            var start = DateTime.Now;
            if(Static.Config.Logging.LogClientPatching) Console.Write($"[Client Patcher] Patching file {file}...");
            var contents = File.ReadAllText(file);
            contents = AssetsController.PatchClient(contents);
            File.WriteAllText(file, contents);
            if(Static.Config.Logging.LogClientPatching) Console.WriteLine($" Done in {DateTime.Now - start}!");
        }

        if (Static.Config.Api.Debug.ReformatAssets)
        {
            Console.WriteLine("[Client Patcher] Reformatting assets...");
            int baseprocs = Process.GetProcessesByName("node").Length;
            foreach (var file in Directory.GetFiles(Static.Config.Api.AssetCache.DiskCachePath))
            {
                var target = file.Replace(Static.Config.Api.AssetCache.DiskCachePath, Static.Config.Api.Debug.FormattedAssetPath);
                if(!File.Exists(target))
                    File.Copy(file, target, false);
                //var proc = Process.Start("npx", "prettier -w " + target);//.WaitForExit(1000);
                //proc.WaitForExit(500);
                //if (Process.GetProcessesByName("node").Length > baseprocs + 10) proc.WaitForExit();  //Thread.Sleep(1000);
            }

            Process.Start("npx", "prettier -w " + Static.Config.Api.Debug.FormattedAssetPath).WaitForExit();
            Console.WriteLine("[Client Patcher] Done!");
            if (Static.Config.Api.Debug.OpenFormattedDirAfterReformat)
                Process.Start(Static.Config.Api.Debug.OpenFormattedDirCommand.Command, Static.Config.Api.Debug.OpenFormattedDirCommand.Args.Replace("$dir", Static.Config.Api.Debug.FormattedAssetPath));
        }
    }
}