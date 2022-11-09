using Fosscord.DbModel.Entities;
using Fosscord.Util.Db.Schemas;

namespace Fosscord.Util.Db.ObjectBuilders;

public class RoleBuilder : GenericObjectBuilder<Role>
{
    public async Task<Role> CreateAsync(RoleCreateSchema ccsc)
    {
        var role = new Role()
        {
            Color = ccsc.Color,
            GuildId = ccsc.GuildId.ToString(),
            Hoist = ccsc.Hoist,
            Id = ccsc.Id ?? GenerateId(),

        };
        await db.SaveChangesAsync();
        return role;
    }

    public RoleBuilder(DbModel.Db db) : base(db)
    {
    }
}