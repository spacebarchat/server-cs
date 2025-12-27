using System.Diagnostics;
using ArcaneLibs;
using ReferenceClientProxyImplementation.Configuration;

namespace ReferenceClientProxyImplementation.Patches.Implementations;

public partial class FormatHtmlCssPatch(ProxyConfiguration config) : IPatch {
    public int GetOrder() => -100;

    public string GetName() => "Format HTML/CSS file";
    public bool Applies(string relativeName, byte[] content) => relativeName.EndsWith(".css") || relativeName.EndsWith(".html");

    public async Task<byte[]> Execute(string relativeName, byte[] content) {
        var cachePath = Path.Combine(config.TestClient.RevisionPath, "formatted", relativeName);
        if (File.Exists(cachePath)) {
            Console.WriteLine($"Using cached formatted file for {relativeName}");
            return await File.ReadAllBytesAsync(cachePath);
        }

        Directory.CreateDirectory(Path.GetDirectoryName(cachePath)!);
        var tmpPath = $"{Environment.GetEnvironmentVariable("TMPDIR") ?? "/tmp"}/{Random.Shared.NextInt64()}_{Path.GetFileName(relativeName)}";
        await File.WriteAllBytesAsync(tmpPath, content);
        var sw = Stopwatch.StartNew();
        ProcessStartInfo psi = new ProcessStartInfo(config.AssetCache.PrettierPath, $"-w --print-width 240 {tmpPath}") {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        
        using var process = Process.Start(psi);
        if (process == null) {
            throw new InvalidOperationException("Failed to start the formatting process.");
        }

        // var stdout = await process.StandardOutput.ReadToEndAsync();
        // var stderr = await process.StandardError.ReadToEndAsync();
        // await process.WaitForExitAsync();

        Dictionary<ulong, string> stdoutLines = new();
        Dictionary<ulong, string> stderrLines = new();
        
        while (!process.HasExited) {
            while (!process.StandardOutput.EndOfStream) {
                var line = await process.StandardOutput.ReadLineAsync();
                if (line == null) continue;
                stdoutLines[(ulong)sw.ElapsedMilliseconds] = line;
                Console.Write("O");
            }
            
            while (!process.StandardError.EndOfStream) {
                var line = await process.StandardError.ReadLineAsync();
                if (line == null) continue;
                stderrLines[(ulong)sw.ElapsedMilliseconds] = line;
                Console.Write("E");
            }
        }

        // Console.WriteLine($"Formatted {relativeName} in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");

        if (process.ExitCode != 0) {
            Console.WriteLine($"Failed to format {relativeName} in {sw.ElapsedMilliseconds}ms: {process.ExitCode}");
            Console.WriteLine("Standard Output:\n" + string.Join("\n", stdoutLines.OrderBy(kv => kv.Key).Select(kv => $"[{kv.Key}ms] {kv.Value}")));
            Console.WriteLine("Standard Error:\n" + string.Join("\n", stderrLines.OrderBy(kv => kv.Key).Select(kv => $"[{kv.Key}ms] {kv.Value}")));
            throw new Exception($"Failed to format file {relativeName}: {string.Join("\n", stderrLines.OrderBy(kv => kv.Key).Select(kv => kv.Value))}");
        }

        var result = await File.ReadAllBytesAsync(tmpPath);
        File.Move(tmpPath, cachePath);
        return result;
    }
}