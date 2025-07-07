// using System.Diagnostics;
// using ReferenceClientProxyImplementation.Configuration;
// using Spacebar.API.Controllers;
//
// namespace Spacebar.API.Tasks.Startup;
//
// public class PatchClientAssetsTask(ProxyConfiguration proxyConfig) : ITask
// {
//     public int GetOrder()
//     {
//         return 100;
//     }
//
//     public string GetName()
//     {
//         return "Patch client assets";
//     }
//
//     public void Execute()
//     {
//         foreach (var file in Directory.GetFiles(proxyConfig.Api.AssetCache.DiskCachePath).Where(x => x.EndsWith(".js")))
//         {
//             var start = DateTime.Now;
//             if(proxyConfig.Logging.LogClientPatching) Console.Write($"[Client Patcher] Patching file {file}...");
//             var contents = File.ReadAllText(file);
//             contents = AssetsController.PatchClient(contents);
//             File.WriteAllText(file, contents);
//             if(proxyConfig.Logging.LogClientPatching) Console.WriteLine($" Done in {DateTime.Now - start}!");
//         }
//
//         if (proxyConfig.Api.Debug.ReformatAssets)
//         {
//             Console.WriteLine("[Client Patcher] Reformatting assets...");
//             foreach (var file in Directory.GetFiles(proxyConfig.Api.AssetCache.DiskCachePath))
//             {
//                 var target = file.Replace(proxyConfig.Api.AssetCache.DiskCachePath, proxyConfig.Api.Debug.FormattedAssetPath);
//                 if(!File.Exists(target))
//                     File.Copy(file, target, false);
//             }
//
//             Process.Start("npx", "prettier -w " + proxyConfig.Api.Debug.FormattedAssetPath).WaitForExit();
//             Console.WriteLine("[Client Patcher] Done!");
//             if (proxyConfig.Api.Debug.OpenFormattedDirAfterReformat)
//                 Process.Start(proxyConfig.Api.Debug.OpenFormattedDirCommand.Command, proxyConfig.Api.Debug.OpenFormattedDirCommand.Args.Replace("$dir", proxyConfig.Api.Debug.FormattedAssetPath));
//         }
//     }
// }