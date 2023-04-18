using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Spacebar.ConfigModel;
using Spacebar.DbModel;
using Spacebar.DbModel.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Spacebar.Util;

public class JwtAuthenticationManager
{
    private static readonly ConcurrentDictionary<string, User> _users = new();
    private readonly DbModel.Db _db;

    private readonly string _tokenKey = Config.Instance.Security.JwtSecret;

    public JwtAuthenticationManager(DbModel.Db db)
    {
        _db = db;
    }
    public async Task<User> GetUserFromToken(string token, DbModel.Db? db = null)
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
            ValidateIssuer = false
        };
        var tokenClaim = tokenHandler.ValidateToken(token, validationParameters, out var tokenValidated);
        User? user;
        if (_users.ContainsKey(tokenClaim.Identity.Name))
        {
            user = _users[tokenClaim.Identity.Name];
            Console.WriteLine($"User {user.Id} found in cache!");   
        }
        else
        {
            user = await db.Users.FirstOrDefaultAsync(x => x.Id == tokenClaim.Identity.Name);
            Console.WriteLine($"User {user?.Id} not found in cache, fetching from db");
            _users.TryAdd(user.Id, user);
        }

        
        if (_users.Count > Config.Instance.Gateway.AuthCacheSize)
        {
            Console.WriteLine("Auth cache full, removing oldest user");
            _users.TryRemove(user.Id, out _);
        }

        if (user == null) throw new Exception("User not found");
        
        return user;
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