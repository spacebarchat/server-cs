using Spacebar.DbModel;
using Spacebar.Gateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.Gateway.Controllers;

public class GatewayController : Controller
{
    private readonly ILogger<GatewayController> _Logger;
    private readonly Db _db;

    public GatewayController(ILogger<GatewayController> logger, Db db)
    {
        _Logger = logger;
        _db = db;
    }

    [HttpGet("/")]
    public async Task GetWS([FromQuery] string encoding, [FromQuery] int v, [FromQuery] string compress = "")
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            _Logger.LogInformation("Gateway connection attempt: {ConnectionRemoteIpAddress}",
                HttpContext.Connection.RemoteIpAddress);
            var clientSocketInfo = new WebSocketInfo(encoding, v, compress);
            await clientSocketInfo.AcceptWebSocketAsync(HttpContext.WebSockets);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}