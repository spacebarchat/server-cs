using Spacebar.DbModel.Entities;

namespace Spacebar.Util.Db.ObjectBuilders;

public class ChannelBuilder : GenericObjectBuilder<Channel>
{
    public async Task<Channel> CreateAsync(ChannelCreateSchema ccsc)
    {
        var channel = new Channel()
        {
            Id = GenerateId(),
            Name = ccsc.Name ?? "unnamed",
            Guild = ccsc.Guild
        };
        await db.SaveChangesAsync();
        return channel;
    }

    public ChannelBuilder(DbModel.Db db) : base(db)
    {
    }
}