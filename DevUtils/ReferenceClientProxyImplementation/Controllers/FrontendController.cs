using Microsoft.AspNetCore.Mvc;
using ReferenceClientProxyImplementation.Configuration;
using ReferenceClientProxyImplementation.Helpers;
using ReferenceClientProxyImplementation.Patches.Implementations;

namespace ReferenceClientProxyImplementation.Controllers;

[Controller]
[Route("/")]
public class FrontendController(ProxyConfiguration proxyConfiguration, PatchSet patches) : Controller {
    [HttpGet]
    [HttpGet("/app")]
    [HttpGet("/login")]
    [HttpGet("/register")]
    [HttpGet("/channels/@me")]
    [HttpGet("/channels/{*_}")]
    [HttpGet("/shop")]
    [HttpGet("/app/{*_}")]
    [HttpGet("/open")]
    [HttpGet("/settings/{*_}")]
    [HttpGet("/action/{*_}")]
    [HttpGet("/library/{*_}")]
    public async Task<Stream> Home() {
        var patchedPath = Path.Combine(proxyConfiguration.TestClient.RevisionPath, "patched", "app.html");
        if (!System.IO.File.Exists(patchedPath)) {
            var path = Path.Combine(proxyConfiguration.TestClient.RevisionPath, "src", "app.html");
            var patchedContent = await patches.ApplyPatches("app.html", await System.IO.File.ReadAllBytesAsync(path));
            Directory.CreateDirectory(Path.GetDirectoryName(patchedPath)!);
            await System.IO.File.WriteAllBytesAsync(patchedPath, patchedContent);
        }

        return System.IO.File.OpenRead(patchedPath);
        // return null;
        // if (!proxyConfiguration.TestClient.Enabled)
        // return NotFound("Test client is disabled");
        // var html = await System.IO.File.ReadAllTextAsync(proxyConfiguration.TestClient.UseLatest ? "Resources/Pages/index-updated.html" : "Resources/Pages/index.html");
        //
        // //inject debug utilities
        // var debugOptions = proxyConfiguration.TestClient.DebugOptions;
        // if (debugOptions.DumpWebsocketTrafficToBrowserConsole)
        //     html = html.Replace("<!-- preload plugin marker -->",
        //         await System.IO.File.ReadAllTextAsync("Resources/Private/Injections/WebSocketDataLog.html") + "\n<!-- preload plugin marker -->");
        // if (debugOptions.DumpWebsocketTraffic)
        //     html = html.Replace("<!-- preload plugin marker -->",
        //         await System.IO.File.ReadAllTextAsync("Resources/Private/Injections/WebSocketDumper.html") + "\n<!-- preload plugin marker -->");
        //
        // return File(Encoding.UTF8.GetBytes(html), "text/html");
    }

    [HttpGet("/developers")]
    [HttpGet("/developers/{*_}")]
    public async Task<object> Developers() {
        var patchedPath = Path.Combine(proxyConfiguration.TestClient.RevisionPath, "patched", "developers.html");
        if (!System.IO.File.Exists(patchedPath)) {
            var path = Path.Combine(proxyConfiguration.TestClient.RevisionPath, "src", "developers.html");
            var patchedContent = await patches.ApplyPatches("developers.html", await System.IO.File.ReadAllBytesAsync(path));
            Directory.CreateDirectory(Path.GetDirectoryName(patchedPath)!);
            await System.IO.File.WriteAllBytesAsync(patchedPath, patchedContent);
        }

        return System.IO.File.OpenRead(patchedPath);
    }
}
