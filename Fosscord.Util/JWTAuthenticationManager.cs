using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fosscord.DbModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Fosscord.API.Classes;

public class JWTAuthenticationManager
{
    private readonly Db db = Db.GetNewMysql();
 
    private readonly string tokenKey;
 
    public JWTAuthenticationManager()
    {
        tokenKey = FosscordConfig.GetString("security_jwtSecret");
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
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}