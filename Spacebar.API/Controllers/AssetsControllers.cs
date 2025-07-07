using System.Collections.Concurrent;
using Spacebar.API.Helpers;
using Spacebar.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers;

[Controller]
[Route("/")]
public class AssetsController(Db db) : Controller
{
    private readonly Db _db = db;
    private static readonly ConcurrentDictionary<string, byte[]> cache = new();

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
            return File(cache[res], contentType);

        if (System.IO.File.Exists("./Resources/Assets/" + res))
            cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync("./Resources/Assets/" + res));

        return NotFound();
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