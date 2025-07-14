using System.Diagnostics;
using ArcaneLibs;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Patches.Implementations;

public partial class FormatFilePatch(ProxyConfiguration config) : IPatch {
    public int GetOrder() => -100;

    public string GetName() => "Format file";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".js") || relativeName.EndsWith(".css") || relativeName.EndsWith(".html");

    public async Task<byte[]> Execute(string relativeName, byte[] content) {
        var cachePath = Path.Combine(config.TestClient.RevisionPath, "formatted", relativeName);
        if (File.Exists(cachePath)) {
            Console.WriteLine($"Using cached formatted file for {relativeName}");
            return await File.ReadAllBytesAsync(cachePath);
        }

        Directory.CreateDirectory(Path.GetDirectoryName(cachePath)!);
        var tmpPath = $"/tmp/{Random.Shared.NextInt64()}_{Path.GetFileName(relativeName)}";
        await File.WriteAllBytesAsync(tmpPath, content);
        var sw = Stopwatch.StartNew();
        ProcessStartInfo psi;
        
        // Biome doesn't support HTML and struggles with upstream emitting Sass directives
        if (relativeName.EndsWith(".html") || relativeName.EndsWith(".css"))
            psi = new ProcessStartInfo(config.AssetCache.PrettierPath, $"-w --print-width 240 {tmpPath}") {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        else
            psi = new ProcessStartInfo(config.AssetCache.BiomePath, $"format --write --line-width 240 --files-max-size=100000000 {tmpPath}") {
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
            Console.WriteLine($"Failed to format {relativeName} in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");
            Console.WriteLine("Standard Output: " + stdout);
            Console.WriteLine("Standard Error: " + stderr);
            throw new Exception($"Failed to format file {relativeName}: {stderr}");
        }

        var result = await File.ReadAllBytesAsync(tmpPath);
        File.Move(tmpPath, cachePath);
        return result;
    }
}