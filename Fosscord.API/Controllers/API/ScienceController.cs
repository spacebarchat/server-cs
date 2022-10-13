using Fosscord.API.Classes;
using Fosscord.API.Schemas;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using IdGen;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fosscord.API.Controllers.API;

[Controller]
[Route("/")]
public class ScienceController : Controller
{
    private static readonly Random Rnd = new();
    private readonly JwtAuthenticationManager _auth;
    private readonly Db _db;

    public ScienceController(Db db, JwtAuthenticationManager auth)
    {
        _db = db;
        _auth = auth;
    }

    [HttpPost("/api/science")]
    public async Task<object> CreateGuildAsync()
    {
        return Ok();
    }
}