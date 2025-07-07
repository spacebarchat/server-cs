using System.Text;
using Microsoft.AspNetCore.Mvc;
using ReferenceClientProxyImplementation.Configuration;
using Spacebar.API.Helpers;

namespace ReferenceClientProxyImplementation.Controllers;

[Controller]
[Route("/")]
public class FrontendController(ProxyConfiguration proxyConfiguration) : Controller {
    [HttpGet]
    [HttpGet("/app")]
    [HttpGet("/login")]
    [HttpGet("/register")]
    [HttpGet("/channels/@me")]
    public async Task<object> Home() {
        return null;
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
    public async Task<object> Developers() {
        if (proxyConfiguration.TestClient.Enabled)
            // return Resolvers.ReturnFileWithVars("Resources/Pages/developers.html", _db);
            return Resolvers.ReturnFileWithVars("Resources/Pages/developers.html", []);
        return NotFound("Test client is disabled");
    }
}