using Microsoft.AspNetCore.Mvc;
using Spacebar.Util;

namespace Spacebar.API.Controllers.API.Auth;

//[Authorize]
[Controller]
[Route("/")]
public class AuthController(Db db, JwtAuthenticationManager auth) : Controller {
    private static readonly Random Rnd = new();

    /// <summary>
    ///     Register a new user
    /// </summary>
    /// <returns>Json object with token</returns>
    [HttpPost("/api/auth/register")]
    public async Task<object> Register([FromBody] RegisterData data) {
        var discrim = Rnd.Next(10000).ToString().PadLeft(4, '0');
        if (data.Email is null or "") return new StatusCodeResult(400);
        if (db.Users.Any(x => x.Email == data.Email)) return new StatusCodeResult(403);

        var user = new User {
            Username = data.Username,
            Discriminator = discrim,
            Id = new IdGen.IdGenerator(0).CreateId() + "",
            Email = data.Email,
            Data = new UserData {
                Hash = BCrypt.Net.BCrypt.HashPassword(data.Password, 12),
                ValidTokensSince = DateTime.Now
            },
            Settings = new UserSetting()
        };
        user.Settings.Id = user.Id;
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var token = auth.Authenticate(data.Email, data.Password);
        if (token == null) return new StatusCodeResult(500);
        Console.WriteLine("WE REACHED END OF APP LOGIC!!!!!!!!!!");
        return new { token };
    }

    /// <summary>
    ///     Log a user in
    /// </summary>
    /// <returns>Json object with token</returns>
    [HttpPost("/api/auth/login")]
    public async Task<object> Login([FromBody] LoginData data) {
        var token = auth.Authenticate(data.Login, data.Password);
        if (token == null) return StatusCode(403, "Invalid username or password!");
        return new { token };
    }
}