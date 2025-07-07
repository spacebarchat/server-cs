using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Spacebar.Static.Enums;

namespace Spacebar.API.Controllers.Tools;

[Controller]
[Route("/tools/rights/")]
public class RightsCalculatorController : Controller {
    [HttpGet]
    public async Task<ContentResult> ToolsIndex() {
        var rights = typeof(Rights).GetFields();
        var html = "<h1>Spacebar server rights</h1>\n" +
                   "<style>p{margin-block-start:0px;}</style>\n" +
                   "<div id=\"rights\">\n";
        foreach (var fieldInfo in rights) {
            var description = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            html +=
                $"<input type=\"checkbox\" name=\"{fieldInfo.Name}\" value=\"{fieldInfo.GetValue(null)}\"onclick=\"calculate(this)\">{fieldInfo.Name} [1<<{fieldInfo.GetValue(null)}]</input>" +
                $"<p>({description?.Description ?? "No description!"})</p>\n";
        }

        html += "</div>\n" +
                "<p id=\"legacyRights\">Legacy rights (Spacebar server-ts): </p>\n" +
                "<p id=\"modernRights\">User rights: </p>\n" +
                "<p>Config rights:</p>\n" +
                "<textarea id=\"configRights\" cols=\"50\" rows=\"10\"></textarea>" +
                "<script src=\"/assets/tools/rightsCalculator.js\"></script>";

        return new ContentResult {
            ContentType = "text/html",
            StatusCode = 200,
            Content = html
        };
    }
}