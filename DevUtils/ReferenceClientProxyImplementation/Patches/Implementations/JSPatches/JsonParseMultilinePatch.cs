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

        

        await Parallel.ForEachAsync(matches, async (match, ct) => {
            string formattedJson = match.Groups[1].Value;
            try {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(formattedJson.Replace("\\", "\\\\"));
                formattedJson = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
            } catch (JsonException je) {
                Console.WriteLine($"STJ: Failed to parse JSON in {relativePath} at index {match.Index}: {je.Message}");
                try {
                    formattedJson = await formatJsonWithNodejs(relativePath, match, ct);
                } catch (Exception e) {
                    Console.WriteLine($"Node.js: Failed to parse JSON in {relativePath} at index {match.Index}: {e.Message}");
                    return;
                }
            }
            
            lock (matches) stringContent = stringContent.Replace(match.Value, $"JSON.parse(`{formattedJson.Replace("\\", "\\\\")}`);");
        });

        return Encoding.UTF8.GetBytes(stringContent);
    }
    
    private async Task<string> formatJsonWithNodejs(string relativePath, Match match, CancellationToken ct) {
            // Extract the JSON string from the match
            var id = "dcp_" + Path.GetFileName(relativePath).Replace('.', '_') + "_" + match.Index;
            await File.WriteAllTextAsync($"{Environment.GetEnvironmentVariable("TMPDIR") ?? "/tmp"}/{id}.js", $"console.log(JSON.stringify(JSON.parse(`{match.Groups[1].Value.Replace("`", "\\\\\\`")}`), null, 2))");
            var sw = Stopwatch.StartNew();

            var psi = new ProcessStartInfo(config.AssetCache.NodePath, $"{Environment.GetEnvironmentVariable("TMPDIR") ?? "/tmp"}/{id}.js") { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true };

            using var process = Process.Start(psi);
            if (process == null) {
                throw new InvalidOperationException("Failed to start the formatting process.");
            }

            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
            // Console.WriteLine($"Formatted {relativeName} in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");

            if (process.ExitCode != 0) {
                Console.WriteLine($"Failed to run {Environment.GetEnvironmentVariable("TMPDIR") ?? "/tmp"}/{id}.js in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");
                Console.WriteLine("Standard Output: " + stdout);
                Console.WriteLine("Standard Error: " + stderr);
                throw new Exception($"Failed to execute {Environment.GetEnvironmentVariable("TMPDIR") ?? "/tmp"}/{id}.js: {stderr}");
            }

            var formattedJson = stdout.Trim();
            Console.WriteLine($"Parsed JSON({id}) in {sw.ElapsedMilliseconds}ms: {formattedJson.Length} bytes");
            // stringContent = stringContent.Replace(match.Value, $"JSON.parse(`{formattedJson.Replace("\\n", "\\\\n")}`);");
            await File.WriteAllTextAsync($"{config.TestClient.RevisionPath}/patched/assets/{Path.GetFileName(relativePath)}-{match.Index}.json", formattedJson);
            return formattedJson;
    }

    [GeneratedRegex(@"JSON\.parse\(\n\s*'(.*?)',?\s*\);", RegexOptions.Compiled | RegexOptions.Multiline)]
    private static partial Regex JsonParseRegex();
}