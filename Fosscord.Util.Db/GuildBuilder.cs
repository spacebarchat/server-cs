using Fosscord.DbModel.Entities;
using IdGen;

namespace Fosscord.Util.Db;

public class GuildBuilder
{
    public static string GenerateId()
    {
        return new IdGenerator(Environment.CurrentManagedThreadId).CreateId() + "";
    }
    public static async Task<Guild> CreateGuildAsync(DbModel.Db? db = null, string? name = null, User? owner = null, string? region = null)
    {
        db ??= DbModel.Db.GetNewDb();
        var guild = new Guild
        {
            Id = GenerateId(),
            Name = name ?? "Unnamed guild",
            Region = region??"",
            Icon = "",
            WelcomeScreen = new()
        };
        await db.SaveChangesAsync();
        guild.RulesChannel = await CreateChannelAsync(db, "rules", guild);
        guild.SystemChannel = await CreateChannelAsync(db, "system", guild);
        guild.PublicUpdatesChannel = await CreateChannelAsync(db, "public-updates", guild);
        await db.SaveChangesAsync();
        return guild;
    }

    public static async Task<Channel> CreateChannelAsync(DbModel.Db? db = null, string? name = null, Guild? guild = null)
    {
        db ??= DbModel.Db.GetDb();
        var channel = new Channel()
        {
            Id = new IdGenerator(Environment.CurrentManagedThreadId).CreateId() + "",
            Name = name ?? "unnamed",
            Guild = guild
        };
        await db.SaveChangesAsync();
        return channel;
    }
}