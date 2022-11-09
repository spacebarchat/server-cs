using Fosscord.DbModel.Entities;
using Fosscord.Util.Schemas;

namespace Fosscord.Util.Db.ObjectBuilders;

public class ChannelCreateSchema
{
    public string Name { get; set; }
    public ChannelType Type { get; set; }
    public Guild? Guild;
}