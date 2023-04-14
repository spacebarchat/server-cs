using System.Text;
using Spacebar.API.Helpers;
using Spacebar.ConfigModel;
using Spacebar.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers;

[Controller]
[Route("/")]
public class FrontendController : Controller
{
    private readonly Db _db;

    public FrontendController(Db db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<object> Home()
    {
        var html = await System.IO.File.ReadAllTextAsync("Resources/Pages/index.html");
        return File(Encoding.UTF8.GetBytes(html), "text/html");
    }
}