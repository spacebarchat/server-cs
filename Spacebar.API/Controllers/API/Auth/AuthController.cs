using Spacebar.DbModel;
using Spacebar.DbModel.Classes;
using Spacebar.DbModel.Entities;
using Spacebar.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Spacebar.API.Controllers.API.Auth;

//[Authorize]
[Controller]
[Route("/")]
public class AuthController : Controller
{
    private readonly Db _db;
    private readonly JwtAuthenticationManager _auth;
    private static readonly Random Rnd = new();

    public AuthController(Db db, JwtAuthenticationManager auth)
    {
        _db = db;
        _auth = auth;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <returns>Json object with token</returns>
    [HttpPost("/api/auth/register")]
    public async Task<object> Register([FromBody] RegisterData data)
    {
        var discrim = Rnd.Next(10000).ToString().PadLeft(4, '0');
        if (data.Email is null or "") return new StatusCodeResult(400);
        if (_db.Users.Any(x => x.Email == data.Email)) return new StatusCodeResult(403);

        var user = new User()
        {
            Username = data.Username,
            Discriminator = discrim,
            Id = new IdGen.IdGenerator(0).CreateId() + "",
            Email = data.Email,
            Data = new UserData
            {
                Hash = BCrypt.Net.BCrypt.HashPassword(data.Password, 12),
                ValidTokensSince = DateTime.Now
            },
            Settings = new UserSetting()
        };
        user.Settings.Id = user.Id;
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _auth.Authenticate(data.Email, data.Password);
        if (token == null) return new StatusCodeResult(500);
        Console.WriteLine("WE REACHED END OF APP LOGIC!!!!!!!!!!");
        return new { token };
    }

    /// <summary>
    /// Log a user in
    /// </summary>
    /// <returns>Json object with token</returns>
    [HttpPost("/api/auth/login")]
    public async Task<object> Login([FromBody] LoginData data)
    {
        var token = _auth.Authenticate(data.Login, data.Password);
        if (token == null) return StatusCode(403, "Invalid username or password!");
        return new { token };
    }
}