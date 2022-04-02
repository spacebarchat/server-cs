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
    public async Task<object> Home()
    {
        if (FosscordConfig.GetBool("client_testClient_latest", false))
            return Resolvers.ReturnFileWithVars("Resources/Pages/index-updated.html", _db);
        return Resolvers.ReturnFileWithVars("Resources/Pages/index.html", _db);
    }
    [HttpGet("/ws")]
    public async Task GetWS()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            //await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}