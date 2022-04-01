using Fosscord.API.Helpers;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers;

[Controller]
[Route("/")]
public class FrontendController : Controller {
    private readonly Db _db;

    public FrontendController(Db db) {
        _db = db;
    }

    [HttpGet]
    [HttpGet("/login")]
    [HttpGet("/register")]
    public object Home() {
        return Resolvers.ReturnFileWithVars("Resources/Pages/index.html", _db);
    }
}