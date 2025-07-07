// using System.Net;
// using AngleSharp.Html.Parser;
//
// namespace ReferenceClientProxyImplementation.Services;
//
// public class BuildDownloadService(ILogger<BuildDownloadService> logger) {
//     private static readonly HttpClient hc = new();
//
//     public async Task DownloadBuildFromArchiveOrg(string outputDirectory, DateTime timestamp) {
//         // 20150906025145
//         var paddedTimestamp = timestamp.ToString("yyyyMMddHHmmss");
//         await DownloadBuildFromUrl(outputDirectory, $"https://web.archive.org/web/{paddedTimestamp}im_/https://discordapp.com/login");
//     }
//
//     public async Task DownloadBuildFromUrl(string outputDirectory, string url) {
//         logger.LogInformation("Downloading build from {url} to {outDir}", url, outputDirectory);
//         var response = await hc.GetAsync(url);
//         if (!response.IsSuccessStatusCode)
//             throw new Exception($"Failed to download build from {url}");
//         var html = await response.Content.ReadAsStringAsync();
//         File.WriteAllText(outputDirectory + "/index.html", html);
//         var parser = new HtmlParser();
//         var document = parser.ParseDocument(html);
//         var assets = document.QuerySelectorAll("link[rel=stylesheet], link[rel=icon], script, img");
//         foreach (var asset in assets) {
//             var assetUrl = asset.GetAttribute("href") ?? asset.GetAttribute("src");
//             if (assetUrl == null)
//                 continue;
//             if (assetUrl.StartsWith("//")) {
//                 logger.LogWarning("Skipping asset {assetUrl} as it is a protocol-relative URL", assetUrl);
//                 continue;
//             }
//
//             var assetStream = await GetAssetStream(assetUrl);
//             var assetPath = Path.Combine(outputDirectory, assetUrl.TrimStart('/'));
//             Console.WriteLine($"Downloading asset {assetUrl} to {assetPath}");
//             Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
//             await using var fs = File.Create(assetPath);
//             await assetStream.CopyToAsync(fs);
//         }
//
//         logger.LogInformation("Downloading build from {url} complete!", url);
//     }
//
//     public async Task<Stream> GetAssetStream(string asset) {
//         asset = asset.Replace("/assets/", "");
//         var urlsToTry = new Stack<string>(new[] {
//             $"https://web.archive.org/web/0id_/https://discordapp.com/assets/{asset}",
//             $"https://web.archive.org/web/0id_/https://discord.com/assets/{asset}",
//             $"https://discord.com/assets/{asset}"
//         });
//         while (urlsToTry.TryPop(out var urlToTry)) {
//             if (string.IsNullOrWhiteSpace(urlToTry)) continue;
//             try {
//                 var response = await hc.GetAsync(urlToTry, HttpCompletionOption.ResponseHeadersRead);
//                 if (response.IsSuccessStatusCode) {
//                     Console.WriteLine($"Got success for asset {asset} from {urlToTry}");
//                     return await response.Content.ReadAsStreamAsync();
//                 }
//                 //redirect
//
//                 if (response.StatusCode == HttpStatusCode.Found) {
//                     var redirectUrl = response.Headers.Location?.ToString();
//                     if (string.IsNullOrWhiteSpace(redirectUrl)) continue;
//                     urlsToTry.Push(redirectUrl);
//                 }
//                 else logger.LogWarning("Failed to download asset {asset} from {urlToTry}", asset, urlToTry);
//             }
//             catch {
//                 // ignored
//             }
//         }
//
//         throw new Exception($"Failed to download asset {asset}");
//     }
// }