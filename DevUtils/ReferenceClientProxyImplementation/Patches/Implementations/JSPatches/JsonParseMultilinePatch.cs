using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Patches.Implementations.JSPatches;

public partial class JsonParseMultilinePatch(ProxyConfiguration config) : IPatch {
    public int GetOrder() => 1;

    public string GetName() => "Patch null-coalescing expressions in JS";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js");

    public async Task<byte[]> Execute(string relativePath, byte[] content) {
        var stringContent = Encoding.UTF8.GetString(content);

        var matches = JsonParseRegex().Matches(stringContent);
        Console.WriteLine($"Found {matches.Count} JSON.parse calls in {relativePath}");
        foreach (Match match in matches) {
            // Extract the JSON string from the match
            var id = Guid.NewGuid().ToString();
            await File.WriteAllTextAsync($"/tmp/{id}.js", $"console.log(JSON.stringify(JSON.parse(`{match.Groups[1].Value}`), null, 2))");
            var sw = Stopwatch.StartNew();

            var psi = new ProcessStartInfo(config.AssetCache.NodePath, $"/tmp/{id}.js") {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            if (process == null) {
                throw new InvalidOperationException("Failed to start the formatting process.");
            }

            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
            // Console.WriteLine($"Formatted {relativeName} in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");

            if (process.ExitCode != 0) {
                Console.WriteLine($"Failed to run /tmp/{id}.js in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");
                Console.WriteLine("Standard Output: " + stdout);
                Console.WriteLine("Standard Error: " + stderr);
                throw new Exception($"Failed to execute /tmp/{id}.js: {stderr}");
            }
            
            var formattedJson = stdout.Trim();
            Console.WriteLine($"Parsed JSON in {sw.ElapsedMilliseconds}ms: {formattedJson.Length} bytes");
            // stringContent = stringContent.Replace(match.Value, $"JSON.parse(`{formattedJson.Replace("\\n", "\\\\n")}`);");
            await File.WriteAllTextAsync($"{config.TestClient.RevisionPath}/patched/assets/{Path.GetFileName(relativePath)}-{match.Index}.json", formattedJson);
            stringContent = stringContent.Replace(match.Value, $"JSON.parse(`{formattedJson.Replace("\\", "\\\\")}`);");
        }

        return Encoding.UTF8.GetBytes(stringContent);
    }

    [GeneratedRegex(@"JSON\.parse\(\n\s*'(.*?)',?\s*\);", RegexOptions.Compiled | RegexOptions.Multiline)]
    private static partial Regex JsonParseRegex();
}