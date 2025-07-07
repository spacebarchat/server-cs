using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Spacebar.ConfigModel;
using Spacebar.DbModel.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Spacebar.Util;

public class JwtAuthenticationManager(DbModel.Db db, Config config)
{
    private static readonly ConcurrentDictionary<string, User> _users = new();

    private readonly string _tokenKey = config.Security.JwtSecret;

    public async Task<User> GetUserFromToken(string token, DbModel.Db? db1 = null)
    {
        db1 ??= db;
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
        var userId = tokenClaim.Identity?.Name ?? (tokenValidated as JwtSecurityToken)?.Claims.Where(x=>x.Type == "name").Select(x=>x.Value).FirstOrDefault();
        if (_users.ContainsKey(userId))
        {
            user = _users[userId];
            Console.WriteLine($"User {user.Id} found in cache!");   
        }
        else
        {
            user = await db1.Users.FirstOrDefaultAsync(x => x.Id == userId);
            Console.WriteLine($"User {user?.Id} not found in cache, fetching from db");
            _users.TryAdd(user.Id, user);
        }

        
        if (_users.Count > config.Gateway.AuthCacheSize)
        {
            Console.WriteLine("Auth cache full, removing oldest user");
            _users.TryRemove(user.Id, out _);
        }

        if (user == null) throw new Exception("User not found");
        
        return user;
    }

    public string? Authenticate(string username, string password)
    {
        var user = db.Users.FirstOrDefault(x => x.Email == username);
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