using Spacebar.Util.Schemas;

namespace Spacebar.Util.Db.Schemas;

public class RoleCreateSchema : IRequestSchema {
    internal string? Id { get; set; }
    public string Name { get; set; }
    public int Color { get; set; }
    public bool Hoist { get; set; }
    public bool Mentionable { get; set; }
    public int Position { get; set; }
    public string GuildId { get; set; }
    public ulong? Permissions { get; set; }
}