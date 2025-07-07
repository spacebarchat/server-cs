using Microsoft.AspNetCore.Mvc;
using Spacebar.API.Helpers;

namespace Spacebar.API.Controllers;

[Controller]
[Route("/")]
public class StaticController(Db db) : Controller {
    private readonly Db _db = db;

    [HttpGet("/resources/{*res:required}")]
    public object Resource(string res) {
        if (System.IO.File.Exists("./Resources/Static/" + res))
            return Resolvers.ReturnFile("./Resources/Static/" + res);
        return new RedirectResult("https://discord.gg/assets/" + res);
    }
}