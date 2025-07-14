using Microsoft.AspNetCore.Mvc;
using ReferenceClientProxyImplementation.Configuration;
using ReferenceClientProxyImplementation.Services;

namespace ReferenceClientProxyImplementation.Controllers;

[Controller]
[Route("/")]
public class AssetsController(ProxyConfiguration proxyConfiguration, ClientStoreService clientStore) : Controller {
    [HttpGet("/assets/{*res:required}")]
    public async Task<IActionResult> Asset(string res) {
        if (res == "version.staging.json" || res.EndsWith(".map")) {
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

        return new FileStreamResult(await clientStore.GetPatchedClientAsset("assets/" + res), contentType);
        // return ;
    }
}