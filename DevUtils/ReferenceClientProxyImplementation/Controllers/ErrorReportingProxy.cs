using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace ReferenceClientProxyImplementation.Controllers;

[Controller]
[Route("/")]
public class ErrorReportingProxy : Controller {
    [HttpPost("/error-reporting-proxy/web")]
    public async Task<ActionResult> HandleErrorReport() {
        // read body as string
        var body = await new StreamReader(Request.Body).ReadToEndAsync();
        var lines = body.Split('\n');
        var data = new JsonObject() {
            ["eventInfo"] = JsonSerializer.Deserialize<JsonObject>(lines[0]),
            ["typeInfo"] = JsonSerializer.Deserialize<JsonObject>(lines[1]),
            ["stackTrace"] = JsonSerializer.Deserialize<JsonObject>(lines[2]),
        };

        if (lines.Length > 3)
            for (var i = 3; i < lines.Length; i++) {
                data[$"unk_line_{i}"] = JsonSerializer.Deserialize<JsonValue>(lines[i]);
            }
        
        if (!System.IO.Directory.Exists("error_reports"))
            System.IO.Directory.CreateDirectory("error_reports");
        await System.IO.File.WriteAllTextAsync($"error_reports/web_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.json", data.ToJsonString(new JsonSerializerOptions {
            WriteIndented = true
        }));

        return NoContent();
    }
}