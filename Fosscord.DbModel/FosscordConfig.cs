using System.Linq;
using Fosscord.DbModel.Scaffold;

namespace Fosscord.DbModel;

public class FosscordConfig
{
    public static Db db = Db.GetNewPostgres();

    public static int GetInt(string key, int defaultValue = 0)
    {
        var val = db.Configs.FirstOrDefault(x => x.Key == key)?.Value;
        
        if (val == null)
        {
            val = defaultValue+"";
            db.Configs.Add(new Config()
            {
                Key = key,
                Value = defaultValue+""
            });
        }

        db.SaveChanges();
        
        return int.Parse(val);
    }

    public static string GetString(string key, string defaultValue = "")
    {
        var val = db.Configs.FirstOrDefault(x => x.Key == key)?.Value;
        
        if (val == null)
        {
            val = defaultValue+"";
            db.Configs.Add(new Config()
            {
                Key = key,
                Value = defaultValue+""
            });
        }

        db.SaveChanges();
        
        return val;
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        var val = db.Configs.FirstOrDefault(x => x.Key == key)?.Value;
        
        if (val == null)
        {
            val = defaultValue+"";
            db.Configs.Add(new Config()
            {
                Key = key,
                Value = defaultValue+""
            });
        }

        db.SaveChanges();
        
        return bool.Parse(val);
    }
}