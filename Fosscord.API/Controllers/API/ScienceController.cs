using Fosscord.DbModel;
using Fosscord.Util;
using Microsoft.AspNetCore.Mvc;

namespace Fosscord.API.Controllers.API;

[Controller]
[Route("/")]
public class ScienceController : Controller
{
    [HttpPost("/api/science")]
    public async Task<object> CreateGuildAsync()
    {
        return Ok();
    }
}