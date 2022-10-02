using Fosscord.API.Classes;
using Fosscord.API.PostData;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fosscord.API.Controllers.API.Auth;

//[Authorize]
[Controller]
[Route("/")]
public class AuthController : Controller
{
    private readonly Db _db;
    private readonly JWTAuthenticationManager _auth;
    private static readonly Random rnd = new Random();
    
    public AuthController(Db db, JWTAuthenticationManager auth)
    {
        _db = db;
        _auth = auth;
    }

    [HttpPost("/api/auth/register")]
    public async Task<object> Register()
    {
        var data = JsonConvert.DeserializeObject<RegisterData>(await new StreamReader(Request.Body).ReadToEndAsync());
        Console.WriteLine(JsonConvert.SerializeObject(data));
        string discrim = rnd.Next(10000).ToString();
        if (_db.Users.Any(x => x.Email == data.Email)) return new StatusCodeResult(403);
        var user = new User()
        {
            CreatedAt = DateTime.Now,
            Username = data.Username,
            Discriminator = discrim,
            Id = new IdGen.IdGenerator(0).CreateId() + "",
            Bot = false,
            System = false,
            Desktop = false,
            Mobile = false,
            Premium = true,
            PremiumType = 2,
            Bio = "",
            MfaEnabled = false,
            Verified = true,
            Disabled = false,
            Deleted = false,
            Email = data.Email,
            Rights = 0, // TODO = grant rights correctly, as 0 actually stands for no rights at all
            NsfwAllowed = true, // TODO = depending on age
            PublicFlags = 0,
            Flags = "0", // TODO = generate
            Data = JsonConvert.SerializeObject(new
            {
                hash = BCrypt.Net.BCrypt.HashPassword(data.Password, 12),
                valid_tokens_since = DateTime.Now,
            }),
            Settings = new(),
            Fingerprints = "",
        };
        _db.Users.Add(user);
        _db.SaveChanges();
        
        var token = _auth.Authenticate(data.Email, data.Password);
        if (token == null) return new StatusCodeResult(500);
        return new {token = token};
    }
    [HttpPost("/api/auth/login")]
    public async Task<object> Login()
    {
        var data = JsonConvert.DeserializeObject<LoginData>(await new StreamReader(Request.Body).ReadToEndAsync());
        Console.WriteLine(JsonConvert.SerializeObject(data));
        
        var token = _auth.Authenticate(data.Login, data.Password);
        if (token == null) return new StatusCodeResult(403);
        return new {token = token};
    }
}