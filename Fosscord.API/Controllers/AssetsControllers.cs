using System.Collections.Concurrent;
using System.Net;
using System.Text;
using Fosscord.API.Helpers;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers;

[Controller]
[Route("/")]
public class AssetsController : Controller
{
    private readonly Db _db;
    private static readonly ConcurrentDictionary<string, byte[]> cache = new();

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
            cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync($"{Static.Config.Api.AssetCache.DiskCachePath}/{res}"));
            //return Resolvers.ReturnFile("./cache_formatted/" + res);
        }
        else
        {
            if (!Directory.Exists(Static.Config.Api.AssetCache.DiskCachePath)) Directory.CreateDirectory(Static.Config.Api.AssetCache.DiskCachePath);
            if (res.EndsWith(".map")) return NotFound();
            Console.WriteLine($"[Asset cache] Downloading {"https://discord.com/assets/" + res} -> {Static.Config.Api.AssetCache.DiskCachePath}/{res}");
            try
            {
                using (var hc = new HttpClient())
                {
                    var resp = await hc.GetAsync("https://discord.com/assets/" + res);
                    
                    if (!resp.IsSuccessStatusCode) return NotFound();
                    //save to file
                    var bytes = await resp.Content.ReadAsByteArrayAsync();
                    //check if cloudflare
                    if (bytes.Length == 0)
                    {
                        Console.WriteLine($"[Asset cache] Cloudflare detected, retrying {"https://discord.com/assets/" + res} -> {Static.Config.Api.AssetCache.DiskCachePath}/{res}");
                        await Task.Delay(1000);
                        resp = await hc.GetAsync("https://discord.com/assets/" + res);
                        if (!resp.IsSuccessStatusCode) return NotFound();
                        bytes = await resp.Content.ReadAsByteArrayAsync();
                    }
                    //check if cloudflare html
                    /*if (bytes.Length < 1000 && bytes.ToList().Contains<byte[]>(Encoding.UTF8.GetBytes("Cloudflare")))
                    {
                        Console.WriteLine($"[Asset cache] Cloudflare detected, retrying {"https://discord.com/assets/" + res} -> ./cache/{res}");
                        await Task.Delay(1000);
                        resp = await hc.GetAsync("https://discord.com/assets/" + res);
                        if (!resp.IsSuccessStatusCode) return NotFound();
                        bytes = await resp.Content.ReadAsByteArrayAsync();
                    }*/
                    if(res.EndsWith(".js") || res.EndsWith(".css"))
                    {
                        //remove sourcemap
                        var str = Encoding.UTF8.GetString(bytes);
                        str = PatchClient(str);
                        bytes = Encoding.UTF8.GetBytes(str);
                    }
                    
                    if(Static.Config.Api.AssetCache.DiskCache) await System.IO.File.WriteAllBytesAsync($"{Static.Config.Api.AssetCache.DiskCachePath}/{res}", bytes);
                    cache.TryAdd(res, bytes);
                }
                //await new WebClient().DownloadFileTaskAsync("https://discord.com/assets/" + res, "./cache/" + res);
                //cache.TryAdd(res, await System.IO.File.ReadAllBytesAsync("./cache/" + res));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        if (cache.ContainsKey(res))
        {
            byte[] result = cache[res];
            if(!Static.Config.Api.AssetCache.MemoryCache) cache.TryRemove(res, out _);
            return File(result, contentType);
        }
        return NotFound();
    }

    public static string PatchClient(string str)
    {
        str = str.Replace("//# sourceMappingURL=", "//# disabledSourceMappingURL=");
        str = str.Replace("e.isDiscordGatewayPlaintextSet=function(){0;return!1};", "e.isDiscordGatewayPlaintextSet=function(){return true};");
        str = str.Replace("console.log(\"%c\"+n.SELF_XSS_", "console.valueOf(n.SELF_XSS_");
        str = str.Replace("console.log(\"%c\".concat(n.SELF_XSS_", "console.valueOf(console.valueOf(n.SELF_XSS_");
        return str;
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