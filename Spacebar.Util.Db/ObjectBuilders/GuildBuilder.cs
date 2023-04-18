using Spacebar.DbModel.Entities;
using Spacebar.Util.Db.Schemas;
using Spacebar.Util.Schemas;

namespace Spacebar.Util.Db.ObjectBuilders;

public class GuildBuilder : GenericObjectBuilder<Guild>
{
    private const Permissions DEFAULT_EVERYONE_PERMISSIONS =
        Permissions.CreateInstantInvite |
        Permissions.AddReactions |
        Permissions.ViewChannel |
        Permissions.SendMessages |
        Permissions.SendTtsMessages |
        Permissions.EmbedLinks |
        Permissions.AttachFiles |
        Permissions.ReadMessageHistory |
        Permissions.MentionEveryone |
        Permissions.UseExternalEmojis |
        Permissions.Connect |
        Permissions.Speak |
        Permissions.UseVad |
        Permissions.ChangeNickname;

    public async Task<Guild> CreateAsync(GuildCreateRequestSchema? gcrs)
    {
        var guild = new Guild
        {
            Id = GenerateId(),
            Name = gcrs.Name ?? "Unnamed guild",
            Region = gcrs.Region ?? "",
            Icon = "",
            WelcomeScreen = new WelcomeScreen()
        };
        db.Guilds.Add(guild);

        await db.SaveChangesAsync();
        //create channels
        var channelBuilder = new ChannelBuilder(db);
        guild.RulesChannel = await channelBuilder.CreateAsync(new ChannelCreateSchema
            { Guild = guild, Name = "rules", Type = ChannelType.GuildText });
        guild.SystemChannel = await channelBuilder.CreateAsync(new ChannelCreateSchema
            { Guild = guild, Name = "general", Type = ChannelType.GuildText });
        guild.PublicUpdatesChannel = await channelBuilder.CreateAsync(new ChannelCreateSchema
            { Guild = guild, Name = "announcements", Type = ChannelType.GuildText });
        //create @everyone role
        var roleBuilder = new RoleBuilder(db);
        guild.Roles.Add(await roleBuilder.CreateAsync(new RoleCreateSchema
        {
            GuildId = guild.Id,
            Name = "@everyone", 
            Color = 0, 
            Permissions = (ulong?)DEFAULT_EVERYONE_PERMISSIONS,
            Position = 0,
            Hoist = true,
            Mentionable = true, 
            Id = guild.Id
        }));
        //await db.SaveChangesAsync();
        //create membership
        var membershipBuilder = new MembershipBuilder(db);
        guild.Members.Add(
            await membershipBuilder.CreateAsync(new MemberCreateSchema { Guild = guild, User = gcrs.User }));

        await db.SaveChangesAsync();
        return guild;
    }

    public GuildBuilder(DbModel.Db db) : base(db)
    {
    }
}