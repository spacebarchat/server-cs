using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers.Admin;

[Controller]
[Route("/admin/")]
public class AdminToolsIndexController : Controller
{
    [HttpGet]
    public async Task<ContentResult> ToolsIndex()
    {
        var classes = GetType().Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Controller)) && (t.Namespace?.StartsWith(GetType().Namespace!) ?? false))
            .ToArray();
        var html = "";
        foreach (var ctrlClass in classes)
        {
            //.Select(t => $"<a href=\"{t.Name}\">{t.Name}</a>")
            //get route attribute
            var routeAttr = ctrlClass.GetCustomAttribute<RouteAttribute>();
            if (routeAttr == null) continue;
            var routeBase = routeAttr.Template.Replace("/tools/", "");
            html +=
                $"<p>-{new string('.', ctrlClass.FullName.Count(c => c == '.') - 4)}<a href=\"{routeBase}\">{ctrlClass.Name}</a><br></p>";
            foreach (var methodInfo in ctrlClass.GetMethods().Where(x => x.ReturnType == typeof(Task<ContentResult>)))
            {
                //get the route attribute
                var routeAttribute = methodInfo.GetCustomAttribute<HttpGetAttribute>();
                if (routeAttribute == null) continue;
                html +=
                    $"<p>-+{new string('.', ctrlClass.FullName.Count(c => c == '.') - 4)}<a href=\"{routeBase}/{routeAttribute.Template}\">{routeAttribute.Template ?? routeAttribute.Name ?? methodInfo.Name}</a><br></p>";
                //html += $"<p>+{new String('.', ctrlClass.FullName.Count(c => c == '.') - 4)}<a href=\"{routeBase}/{methodInfo.Name}\">{methodInfo.Name}</a><br></p>";
            }

            html += "\n";
        }

        return new ContentResult()
        {
            ContentType = "text/html",
            StatusCode = 200,
            Content = html
        };
    }
}