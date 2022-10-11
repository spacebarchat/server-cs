using Fosscord.API.Classes;
using Fosscord.API.PostData;
using Fosscord.API.Schemas;
using Fosscord.DbModel;
using Fosscord.DbModel.Scaffold;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fosscord.API.Controllers.API.Guilds;

[Controller]
[Route("/")]
public class GuildController : Controller
{
    private readonly Db _db;
    private readonly JwtAuthenticationManager _auth;
    private static readonly Random Rnd = new Random();

    public GuildController(Db db, JwtAuthenticationManager auth)
    {
        _db = db;
        _auth = auth;
    }

    [HttpPost("/api/guilds")]
    public async Task<object> CreateGuildAsync()
    {
        var request = JsonConvert.DeserializeObject<GuildCreateSchema>(await new StreamReader(Request.Body).ReadToEndAsync());
        var user = _auth.GetUserFromToken(Request.Headers["Authorization"].ToString().Split(" ").Last(), _db);
        var guildId = new IdGen.IdGenerator(0).CreateId() + "";
        var guild = new Guild()
        {
            Id = new IdGen.IdGenerator(0).CreateId() + "",
            Name = request.Name,
            Region = request.Region,
            Icon = request.Icon,
            WelcomeScreen = "{}",
            SystemChannel = new()
            {
                Id = new IdGen.IdGenerator(0).CreateId() + "",
                Name = "system-messages",
                Type = ChannelType.GuildText is int ? (int) ChannelType.GuildText : 0,
                CreatedAt = DateTime.Now
            },
            RulesChannel = new()
            {
                Id = new IdGen.IdGenerator(0).CreateId() + "",
                Name = "rules",
                Type = ChannelType.GuildText is int ? (int) ChannelType.GuildText : 0,
                CreatedAt = DateTime.Now
            },
            PublicUpdatesChannel = new()
            {
                Id = new IdGen.IdGenerator(0).CreateId() + "",
                Name = "public-updates",
                Type = ChannelType.GuildText is int ? (int) ChannelType.GuildText : 0,
                CreatedAt = DateTime.Now
            }
            //Owner = user,
        };
        await _db.SaveChangesAsync();
        //guild.SystemChannel.GuildId = guild.RulesChannel.GuildId = guild.PublicUpdatesChannel.GuildId = guildId;
        var everyoneRole = new Role()
        {
            Id = guild.Id,
            Name = "@everyone",
            Permissions = "2251804225"
        };
        guild.Roles.Add(everyoneRole);

        var member = new Member()
        {
            JoinedAt = DateTime.Now,
            Guild = guild,
            Id = user.Id,
            Settings = "{}",
            Bio = ""
        };
        guild.Members.Add(member);
        member.Roles.Add(everyoneRole);
        _db.Guilds.Add(guild);
        await _db.SaveChangesAsync();
        return new {id = Convert.ToUInt64(guild.Id)};
    }
}