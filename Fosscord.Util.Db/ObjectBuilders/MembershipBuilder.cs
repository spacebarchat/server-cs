using Fosscord.DbModel.Entities;

namespace Fosscord.Util.Db.ObjectBuilders;

public class MembershipBuilder : GenericObjectBuilder<Member>
{
    public MembershipBuilder(DbModel.Db db) : base(db)
    {
    }

    public async Task<Member> CreateAsync(MemberCreateSchema mcs)
    {
        var member = new Member
        {
            GuildId = mcs.Guild.Id,
            Id = mcs.User.Id,
            Roles = new[]{mcs.Guild.Roles.First(r => r.Id == mcs.Guild.Id)},
            JoinedAt = DateTime.Now,
            PremiumSince = DateTime.Now,
            Deaf = false,
            Mute = false,
            Pending = false,
        };

        await db.Members.AddAsync(member);
        await db.SaveChangesAsync();

        return member;
    }
}

public class MemberCreateSchema
{
    public Guild Guild { get; set; }
    public User User { get; set; }
}