using Spacebar.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers;

[Controller]
[Route("/")]
public class StaticController : Controller {
    [HttpGet("/resources/{*res:required}")]
    public object Resource(string res)
    {
        if (System.IO.File.Exists("./Resources/Static/" + res))
            return Resolvers.ReturnFile("./Resources/Static/" + res);
        return new RedirectResult("https://discord.gg/assets/" + res);
    }
}