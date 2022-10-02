using System.Net;
using Fosscord.API.Helpers;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers;

[Controller]
[Route("/")]
public class AssetsController : Controller
{
    private readonly Db _db;
    private static readonly WebClient wc = new WebClient();
    private static readonly Dictionary<string, byte[]> cache = new();

    public AssetsController(Db db)
    {
        _db = db;
    }

    [HttpGet("/assets/{*res:required}")]
    public async Task<object> Asset(string res)
    {
        var ext = res.Split(".").Last();
        var contentType = ext switch
        {
            //text types
            "html" => "text/html",
            "js" => "text/javascript",
            "css" => "text/css",
            "txt" => "text/plain",
            "csv" => "text/csv",
            //image types
            "apng" => "image/apng",
            "gif" => "image/gif",
            "jpg" => "image/jpeg",
            "png" => "image/png",
            "svg" => "image/svg+xml",
            "webp" => "image/webp",
            "ico" => "image/x-icon",
            _ => "application/octet-stream"
        };
        if (cache.ContainsKey(res))
        {
            return File(cache[res], contentType);
        }

        if (System.IO.File.Exists("./Resources/Assets/" + res))
        {
            cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync("./Resources/Assets/" + res));
            //return Resolvers.ReturnFile("./Resources/Assets/" + res);
        }
        else if (System.IO.File.Exists("./cache_formatted/" + res))
        {
            cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync("./cache_formatted/" + res));
            //return Resolvers.ReturnFile("./cache_formatted/" + res);
        }
        else if (System.IO.File.Exists("./cache/" + res))
        {
            cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync("./cache/" + res));
            //return Resolvers.ReturnFile("./cache_formatted/" + res);
        }
        else
        {
            if (!Directory.Exists("./cache")) Directory.CreateDirectory("./cache");
            if (res.EndsWith(".map")) return NotFound();
            Console.WriteLine($"[Asset cache] Downloading {"https://discord.com/assets/" + res} -> ./cache/{res}");
            new WebClient().DownloadFile("https://discord.com/assets/" + res, "./cache/" + res);
            cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync("./cache/" + res));
        }

        //todo reimplement
        //return Resolvers.ReturnFile("./cache/" + res);
        if (cache.ContainsKey(res)) return File(cache[res], contentType);
        else
            return NotFound();

        //return new RedirectResult("https://discord.gg/assets/" + res);
    }

    [HttpGet("/robots.txt")]
    public object Robots()
    {
        return Resolvers.ReturnFile("./Resources/robots.txt");
    }

    [HttpGet("/favicon.ico")]
    public object Favicon()
    {
        return Resolvers.ReturnFile("./Resources/RunData/favicon.png");
    }
}