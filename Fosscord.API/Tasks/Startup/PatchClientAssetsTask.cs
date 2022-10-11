using System.Net;
using Fosscord.API.Controllers;
using Fosscord.API.Utilities;

namespace Fosscord.API.Tasks.Startup;

public class PatchClientAssetsTask : ITask
{
    public string GetName()
    {
        return "Patch client assets";
    }

    public void Execute()
    {
        foreach (var file in Directory.GetFiles(Static.Config.Api.AssetCache.DiskCachePath).Where(x=>x.EndsWith(".js")))
        {
            var start = DateTime.Now;
            Console.Write($"[Client Patcher] Patching file {file}...");
            var contents = File.ReadAllText(file);
            contents = AssetsController.PatchClient(contents);
            File.WriteAllText(file, contents);
            Console.WriteLine($" Done in {DateTime.Now - start}!");
        }
    }
}