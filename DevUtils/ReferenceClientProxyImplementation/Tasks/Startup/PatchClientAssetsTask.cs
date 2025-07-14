// using ReferenceClientProxyImplementation.Configuration;
//
// namespace ReferenceClientProxyImplementation.Tasks.Startup;
//
// public class PatchClientAssetsTask(ProxyConfiguration proxyConfig) : ITask {
//     public int GetOrder() => 100;
//
//     public string GetName() => "Patch client assets";
//
//     public async Task Execute() {
//         // foreach (var file in Directory.GetFiles(proxyConfig.AssetCache.DiskCachePath).Where(x => x.EndsWith(".js")))
//         // {
//         //     var start = DateTime.Now;
//         //     if(proxyConfig.Logging.LogClientPatching) Console.Write($"[Client Patcher] Patching file {file}...");
//         //     var contents = File.ReadAllText(file);
//         //     contents = AssetsController.PatchClient(contents);
//         //     File.WriteAllText(file, contents);
//         //     if(proxyConfig.Logging.LogClientPatching) Console.WriteLine($" Done in {DateTime.Now - start}!");
//         // }
//         //
//         // if (proxyConfig.Debug.ReformatAssets)
//         // {
//         //     Console.WriteLine("[Client Patcher] Reformatting assets...");
//         //     foreach (var file in Directory.GetFiles(proxyConfig.AssetCache.DiskCachePath))
//         //     {
//         //         var target = file.Replace(proxyConfig.AssetCache.DiskCachePath, proxyConfig.TestClient.DebugOptions.FormattedAssetPath);
//         //         if(!File.Exists(target))
//         //             File.Copy(file, target, false);
//         //     }
//         //
//         //     Process.Start("npx", "prettier -w " + proxyConfig.Debug.FormattedAssetPath).WaitForExit();
//         //     Console.WriteLine("[Client Patcher] Done!");
//         //     if (proxyConfig.Debug.OpenFormattedDirAfterReformat)
//         //         Process.Start(proxyConfig.Debug.OpenFormattedDirCommand.Command, proxyConfig.Debug.OpenFormattedDirCommand.Args.Replace("$dir", proxyConfig.Debug.FormattedAssetPath));
//         // }
//     }
// }