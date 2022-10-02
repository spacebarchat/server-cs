using Fosscord.API.Helpers;
using Fosscord.DbModel;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers;

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
    [HttpGet("/app")]
    [HttpGet("/login")]
    [HttpGet("/register")]
    [HttpGet("/channels/@me")]
    public async Task<object> Home()
    {
        if (Static.Config.TestClient.Debug)
            return Resolvers.ReturnFileWithVars("Resources/Pages/index-dbg.html", _db);
        if (Static.Config.TestClient.UseLatest)
            return Resolvers.ReturnFileWithVars("Resources/Pages/index-updated.html", _db);
        if (Static.Config.TestClient.Enabled)
            return Resolvers.ReturnFileWithVars("Resources/Pages/index.html", _db);
        return NotFound("Test client is disabled");
    }

    [HttpGet("/developers")]
    public async Task<object> Developers()
    {
        if (Static.Config.TestClient.Enabled)
            return Resolvers.ReturnFileWithVars("Resources/Pages/developers.html", _db);
        return NotFound("Test client is disabled");
    }
}