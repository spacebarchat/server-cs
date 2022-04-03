using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fosscord.API.Utilities;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Fosscord.API.Classes;

public class JWTAuthenticationManager
{
    private readonly Db db = Db.GetNewDb();
 
    private readonly string tokenKey;
 
    public JWTAuthenticationManager()
    {
        tokenKey = FosscordConfig.GetString("security_jwtSecret", RandomStringGenerator.Generate(255));
    }

    public User GetUserFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(tokenKey);
        var validationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
        };
        var tokenClaim = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken tokenValidated);
        return db.Users.FirstOrDefault(x => x.Id == tokenClaim.Identity.Name);
    }
 
    public string Authenticate(string username, string password)
    {
        var user = db.Users.FirstOrDefault(x => x.Email == username);
        if (user == null) return null;
        var hash = ((dynamic) JsonConvert.DeserializeObject(user.Data)).hash;
        if (!BCrypt.Net.BCrypt.Verify(password, hash.ToString())) return null;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(tokenKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}