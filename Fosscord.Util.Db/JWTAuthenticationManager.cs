using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fosscord.ConfigModel;
using Fosscord.DbModel;
using Fosscord.DbModel.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Fosscord.Util;

public class JwtAuthenticationManager
{
    private readonly DbModel.Db _db = DbModel.Db.GetNewDb();
 
    private readonly string _tokenKey = Config.Instance.Security.JwtSecret;

    public User GetUserFromToken(string token, DbModel.Db? db = null)
    {
        db ??= _db;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenKey);
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
 
    public string? Authenticate(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == username);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.Data.Hash)) return null;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Id)
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