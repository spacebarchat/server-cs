using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Helpers;

public static class Resolvers {
    private static readonly string Navbar = File.Exists("Resources/Parts/Navbar.html") ? File.ReadAllText("Resources/Parts/Navbar.html") : "Navbar not found!";

    public static object ReturnFile(string path) {
        if (!File.Exists(path)) return new NotFoundObjectResult("File doesn't exist!");
        var ext = path.Split(".").Last();
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
            _ => "application/octet-stream"
        };
        switch (ext) {
            case "html":
                return new ContentResult
                {
                    ContentType = contentType,
                    Content = File.ReadAllText(path)
                };
            case "js":
            case "css":
            case "txt":
            case "csv":
            case "svg":
                return new ContentResult
                {
                    ContentType = contentType,
                    Content = File.ReadAllText(path)
                };
            case "png":
            case "webp":
            case "jpg":
            case "gif":
            case "apng":
            case "7z":
            case "gz":
            case "tar":
            case "rar":
            case "zip":
            case "webm":
            case "woff":
            case "jar":
            case "mp3":
            case "mp4":
                return new PhysicalFileResult(Path.GetFullPath(path), contentType);
            default:
                Console.WriteLine($"Unsupported filetype: {ext} ({path})");
                return new PhysicalFileResult(Path.GetFullPath(path), "application/octet-stream");
        }
    }

    public static object ReturnFileWithVars(string path, Dictionary<string, object>? customVars = null) {
        if (!File.Exists(path)) return new NotFoundObjectResult(Debugger.IsAttached ? $"File {path} doesn't exist!" : "File doesn't exist!");
        var result = ReturnFile(path);
        if (result.GetType() != typeof(ContentResult)) return result;
        var contentResult = (ContentResult)result;
        contentResult.Content = contentResult.Content?.Replace("$NAVBAR", Navbar);
        if (customVars != null)
            foreach (var (key, value) in customVars) {
                contentResult.Content = contentResult.Content?.Replace(key, value.ToString());
            }

        result = contentResult;

        return result;
    }
}