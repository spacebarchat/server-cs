using Spacebar.DbModel;
using Spacebar.Gateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.Gateway.Controllers;

public class GatewayController : Controller
{
    private readonly ILogger<GatewayController> _Logger;
    private readonly Db _db;
    private readonly WebSocketInfo _webSocketInfo;

    public GatewayController(ILogger<GatewayController> logger, Db db, WebSocketInfo webSocketInfo)
    {
        _Logger = logger;
        _db = db;
        _webSocketInfo = webSocketInfo;
    }

    [HttpGet("/")]
    public async Task GetWS([FromQuery] string encoding, [FromQuery] int v, [FromQuery] string compress = "")
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            _Logger.LogInformation("Gateway connection attempt: {ConnectionRemoteIpAddress}",
                HttpContext.Connection.RemoteIpAddress);
            await _webSocketInfo.WithSettings(encoding, v, compress).AcceptWebSocketAsync(HttpContext.WebSockets);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}