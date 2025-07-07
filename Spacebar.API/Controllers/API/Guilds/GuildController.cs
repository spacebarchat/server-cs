using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Spacebar.Util;
using Spacebar.Util.Db.ObjectBuilders;
using Spacebar.Util.Schemas;

namespace Spacebar.API.Controllers.API.Guilds;

[Controller]
[Route("/")]
public class GuildController(Db db, JwtAuthenticationManager auth) : Controller {
    [HttpPost("/api/guilds")]
    public async Task<object> CreateGuildAsync() {
        var request =
            JsonSerializer.Deserialize<GuildCreateRequestSchema>(
                await new StreamReader(Request.Body).ReadToEndAsync());
        var user = await auth.GetUserFromToken(Request.Headers["Authorization"].ToString().Split(" ").Last(), db);
        request.User = user;
        var guild = await new GuildBuilder(db).CreateAsync(request);
        /*var guildId = new IdGenerator(0).CreateId() + "";
        var guild = new Guild
        {
            Id = new IdGenerator(0).CreateId() + "",
            Name = request.Name,
            Region = request.Region,
            Icon = request.Icon,
            WelcomeScreen = new(),
            SystemChannel = new Channel
            {
                Id = new IdGenerator(0).CreateId() + "",
                Name = "system-messages",
                Type = ChannelType.GuildText is int ? (int) ChannelType.GuildText : 0,
                CreatedAt = DateTime.Now
            },
            RulesChannel = new Channel
            {
                Id = new IdGenerator(0).CreateId() + "",
                Name = "rules",
                Type = ChannelType.GuildText is int ? (int) ChannelType.GuildText : 0,
                CreatedAt = DateTime.Now
            },
            PublicUpdatesChannel = new Channel
            {
                Id = new IdGenerator(0).CreateId() + "",
                Name = "public-updates",
                Type = ChannelType.GuildText is int ? (int) ChannelType.GuildText : 0,
                CreatedAt = DateTime.Now
            }
            //Owner = user,
        };
        await _db.SaveChangesAsync();
        //guild.SystemChannel.GuildId = guild.RulesChannel.GuildId = guild.PublicUpdatesChannel.GuildId = guildId;
        var everyoneRole = new Role
        {
            Id = guild.Id,
            Name = "@everyone",
            Permissions = "2251804225"
        };
        guild.Roles.Add(everyoneRole);

        var member = new Member
        {
            JoinedAt = DateTime.Now,
            Guild = guild,
            Id = user.Id,
            Settings = new(),
            Bio = ""
        };
        guild.Members.Add(member);
        member.Roles.Add(everyoneRole);
        _db.Guilds.Add(guild);
        await _db.SaveChangesAsync();
        */
        return new { id = Convert.ToUInt64(guild.Id) };
    }
}