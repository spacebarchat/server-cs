using Spacebar.DbModel.Entities;
using Spacebar.Util.Schemas;

namespace Spacebar.Util.Db.ObjectBuilders;

public class ChannelCreateSchema {
    public Guild? Guild;
    public string Name { get; set; }
    public ChannelType Type { get; set; }
}