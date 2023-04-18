using Spacebar.DbModel.Entities;
using Spacebar.Util.Db.Schemas;

namespace Spacebar.Util.Db.ObjectBuilders;

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
            Name = ccsc.Name,
            Permissions = ccsc.Permissions.ToString()
        };
        db.Roles.Add(role);
        await db.SaveChangesAsync();
        return role;
    }

    public RoleBuilder(DbModel.Db db) : base(db)
    {
    }
}