// using System.Text;
// using System.Text.RegularExpressions;
// using ReferenceClientProxyImplementation.Services;
//
// namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;
//
// public partial class PrefetchAssetsPatch(IServiceProvider sp) : IPatch {
//     public int GetOrder() => 1000000;
//
//     public string GetName() => "Prefetch assets";
//     public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");
//
//     private static SemaphoreSlim ss = new(2, 2);
//     private static HashSet<string> alreadyKnownAssets = new();
//
//     public async Task<byte[]> Execute(string relativePath, byte[] content) {
//         // Can't inject service due to loop
//         var stringContent = Encoding.UTF8.GetString(content);
//         var matches = PrefetchAssetsRegex().Matches(stringContent);
//
//         Console.WriteLine($"Found {matches.Count} prefetch assets in {relativePath}");
//         if (matches.Count == 0) {
//             return content; // No matches found, return original content
//         }
//
//         var clientStore = sp.GetRequiredService<ClientStoreService>();
//
//         var newAssets = matches
//             .Select(x => x.Groups[1].Value)
//             .Distinct()
//             .Where(x => !clientStore.HasRawAsset(x) && alreadyKnownAssets.Add(x));
//
//         var tasks = newAssets
//             .Select(async match => {
//                 await ss.WaitAsync();
//                 Console.WriteLine($"Discovered prefetch asset in {relativePath}: {match}");
//                 // var patches = sp.GetRequiredService<PatchSet>();
//                 var res = await clientStore.GetOrDownloadRawAsset(match);
//                 await res.DisposeAsync();
//                 ss.Release();
//                 Console.WriteLine($"Prefetched asset {match} in {relativePath}");
//             }).ToList();
//
//         if (tasks.Count == 0) {
//             Console.WriteLine($"No new prefetch assets found in {relativePath}, returning original content.");
//             return content; // No new assets to prefetch, return original content
//         }
//
//         await Task.WhenAny(tasks);
//
//         return content;
//     }
//
//     [GeneratedRegex(@".\.exports = ""((?:[a-z\d/\.]*?)\.\w{2,4})""", RegexOptions.Compiled)]
//     private static partial Regex PrefetchAssetsRegex();
// }