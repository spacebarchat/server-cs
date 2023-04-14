using Spacebar.DbModel;
using Spacebar.Util;
using Microsoft.AspNetCore.Mvc;

namespace Spacebar.API.Controllers.API;

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