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
        if (FosscordConfig.GetBool("client_testClient_debug", false))
            return Resolvers.ReturnFileWithVars("Resources/Pages/index-dbg.html", _db);
        if (FosscordConfig.GetBool("client_testClient_latest", false))
            return Resolvers.ReturnFileWithVars("Resources/Pages/index-updated.html", _db);
        return Resolvers.ReturnFileWithVars("Resources/Pages/index.html", _db);
    }
    
    [HttpGet("/developers")]
    public async Task<object> Developers()
    {
        return Resolvers.ReturnFileWithVars("Resources/Pages/developers.html", _db);
    }
}