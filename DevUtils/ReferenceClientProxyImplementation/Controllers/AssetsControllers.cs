//#define MEMCACHE
using System.Collections.Frozen;
using ArcaneLibs.Extensions.Streams;
using Microsoft.AspNetCore.Mvc;
using ReferenceClientProxyImplementation.Configuration;
using ReferenceClientProxyImplementation.Services;

namespace ReferenceClientProxyImplementation.Controllers;

[Controller]
[Route("/")]
public class AssetsController(ProxyConfiguration proxyConfiguration, ClientStoreService clientStore) : Controller {
#if MEMCACHE
    private static FrozenDictionary<string, ReadOnlyMemory<byte>> memCache = new Dictionary<string, ReadOnlyMemory<byte>>().ToFrozenDictionary();
#endif

    [HttpGet("/assets/{*res:required}")]
    public async Task<IActionResult> Asset(string res) {
        if (res is "version.staging.json" or "version.internal.json")
            res = $"version.{proxyConfiguration.TestClient.Revision}.json";
        else if (res.EndsWith(".map")) {
            return NotFound();
        }

        var ext = res.Split(".").Last();
        var contentType = ext switch {
            //text types
            "html" => "text/html",
            "js" => "text/javascript",
            "css" => "text/css",
            "txt" => "text/plain",
            "csv" => "text/csv",
            "json" => "application/json",
            //image types
            "apng" => "image/apng",
            "gif" => "image/gif",
            "jpg" => "image/jpeg",
            "png" => "image/png",
            "svg" => "image/svg+xml",
            "webp" => "image/webp",
            "ico" => "image/x-icon",
            //script types
            "wasm" => "application/wasm",
            _ => "application/octet-stream"
        };
        // Response.Headers.ContentType = contentType;
#if MEMCACHE
        if (memCache.TryGetValue(res, out var value)) {
            // value.Position = 0;
            var cms = new MemoryStream(value.ToArray());
            // await value.CopyToAsync(cms);
            // ms.Position = 0;
            
            return new FileStreamResult(cms, contentType);
        }
#endif

#if MEMCACHE
        var stream = await clientStore.GetPatchedClientAsset("assets/" + res);
        stream.Position = 0;
        // var ms = new MemoryStream(stream.ReadToEnd().ToArray(), false);
        // memCache = memCache.Append(new KeyValuePair<string, MemoryStream>(res, ms)).ToFrozenDictionary();
        // return new FileStreamResult(ms, contentType);
        var mem = new ReadOnlyMemory<byte>(stream.ReadToEnd().ToArray());
        memCache = memCache.Append(new KeyValuePair<string, ReadOnlyMemory<byte>>(res, mem)).ToFrozenDictionary();
        return new FileStreamResult(new MemoryStream(mem.ToArray()), contentType);
#else
        return new FileStreamResult(await clientStore.GetPatchedClientAsset("assets/" + res), contentType);
#endif
        // return ;
    }
}