using Spacebar.API.Helpers;
using Spacebar.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers;

[Controller]
[Route("/")]
public class StaticController : Controller
{
    private readonly Db _db;

    public StaticController(Db db)
    {
        _db = db;
    }

    [HttpGet("/resources/{*res:required}")]
    public object Resource(string res)
    {
        if (System.IO.File.Exists("./Resources/Static/" + res))
            return Resolvers.ReturnFile("./Resources/Static/" + res);
        return new RedirectResult("https://discord.gg/assets/" + res);
    }
}