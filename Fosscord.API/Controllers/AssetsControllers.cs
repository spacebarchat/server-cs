using System.Net;
using Fosscord.API.Helpers;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers;

[Controller]
[Route("/")]
public class AssetsController : Controller {
    private readonly Db _db;
    private static readonly WebClient wc = new WebClient();
    public AssetsController(Db db) {
        _db = db;
    }
    
    [HttpGet("/assets/{*res:required}")]
    public object Asset(string res)
    {
        if(System.IO.File.Exists("./Resources/Assets/"+res)) return Resolvers.ReturnFile("./Resources/Assets/" + res);
        if (!System.IO.File.Exists("./cache/"+res))
        {
            if (!Directory.Exists("./cache")) Directory.CreateDirectory("./cache");
            if (res.EndsWith(".map")) return NotFound();
            Console.WriteLine($"[Asset cache] Downloading {"https://discord.com/assets/" + res} -> ./cache/{res}");
            new WebClient().DownloadFile("https://discord.com/assets/" + res, "./cache/"+res);
        }
        
        return Resolvers.ReturnFile("./cache/" + res);
        //return new RedirectResult("https://discord.gg/assets/" + res);
    }

    [HttpGet("/robots.txt")]
    public object Robots() {
        return Resolvers.ReturnFile("./Resources/robots.txt");
    }

    [HttpGet("/favicon.ico")]
    public object Favicon() {
        return Resolvers.ReturnFile("./Resources/RunData/favicon.png");
    }
}