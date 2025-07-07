using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers;

[Controller]
[Route("/")]
public class FrontendController(Db db) : Controller {
    private readonly Db _db = db;

    [HttpGet]
    public async Task<object> Home() {
        var html = await System.IO.File.ReadAllTextAsync("Resources/Pages/index.html");
        return File(Encoding.UTF8.GetBytes(html), "text/html");
    }
}