using System.Text;
using Fosscord.API.Helpers;
using Fosscord.ConfigModel;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers;

[Controller]
[Route("/")]
public class FrontendController : Controller
{
    private readonly Db _db;

    public FrontendController(Db db)
    {
        _db = db;
    }

    [HttpGet]
    [HttpGet("/app")]
    [HttpGet("/login")]
    [HttpGet("/register")]
    [HttpGet("/channels/@me")]
    public async Task<object> Home()
    {
        if (!Config.Instance.TestClient.Enabled) 
            return NotFound("Test client is disabled");
        var html = await System.IO.File.ReadAllTextAsync(Config.Instance.TestClient.UseLatest ? "Resources/Pages/index-updated.html" : "Resources/Pages/index.html");

        //inject debug utilities
        var debugOptions = Config.Instance.TestClient.DebugOptions;
        if(debugOptions.DumpWebsocketTrafficToBrowserConsole)
            html = html.Replace("<!-- preload plugin marker -->", await System.IO.File.ReadAllTextAsync("Resources/Private/Injections/WebSocketDataLog.html")+"\n<!-- preload plugin marker -->");
        if(debugOptions.DumpWebsocketTraffic)
            html = html.Replace("<!-- preload plugin marker -->", await System.IO.File.ReadAllTextAsync("Resources/Private/Injections/WebSocketDumper.html")+"\n<!-- preload plugin marker -->");
        
        return File(Encoding.UTF8.GetBytes(html), "text/html");
    }

    [HttpGet("/developers")]
    public async Task<object> Developers()
    {
        if (Config.Instance.TestClient.Enabled)
            return Resolvers.ReturnFileWithVars("Resources/Pages/developers.html", _db);
        return NotFound("Test client is disabled");
    }
}