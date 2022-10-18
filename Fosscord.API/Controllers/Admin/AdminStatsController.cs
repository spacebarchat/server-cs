using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fosscord.API.Controllers.Admin;

[Controller]
[Route("/admin/stats/")]
public class AdminStatsController : Controller
{
    private readonly Db _db;

    public AdminStatsController(Db db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<ContentResult> Stats()
    {
        var start = DateTime.Now;
        string html = "<h1>Fosscord server stats</h1>\n" +
                      "<style>p{margin-block-start:0px; margin-block-end:0px;}</style>\n" +
                      "<div id=\"dbstats\">\n" +
                      "<h2>Database stats:</h2>\n";

        var type = typeof(Db);
        var dbFields = type.GetProperties();
        //get all fields that are DbSet<T>
        foreach (var fieldInfo in dbFields
                     .Where(x=>x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                     .Select(x=>(IQueryable)x.GetValue(_db)!)
                     .ToList()
                     .Select(x=>new {x.ElementType.Name, Count = x.Cast<object>().Count()})
                     .OrderByDescending(x=>x.Count)
                )
        {
            html += $"<p>{fieldInfo.Name}: {fieldInfo.Count} rows</p>\n";
        }
        
        //html += $"<br><p>Generated in {(DateTime.Now - start).TotalMilliseconds}ms</p>";
        html += "</div>";
        html += "<div id=\"1hstats\">\n" +
                "<h2>Last hour stats:</h2>\n";
        html += $"<p>New users: {await _db.Users.CountAsync(x => x.CreatedAt > DateTime.UtcNow.AddHours(-1).ToLocalTime())}\n</p>";
        html += $"<p>New guilds: {await _db.Guilds.CountAsync(x => x.CreatedAt > DateTime.UtcNow.AddHours(-1).ToLocalTime())}</p>";
        html += $"<p>New messages: {await _db.Messages.CountAsync(x => x.Timestamp > DateTime.UtcNow.AddHours(-1).ToLocalTime())}</p>";
        html += "</div>";
        
        
        
        html += $"<br><p>Generated in {(DateTime.Now - start).TotalMilliseconds}ms</p>";
        

        return new ContentResult()
        {
            ContentType = "text/html",
            StatusCode = 200,
            Content = html
        };
    }
}