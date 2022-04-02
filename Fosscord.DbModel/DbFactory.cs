using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Fosscord.DbModel;

public class DbFactory : IDesignTimeDbContextFactory<Db>
{
    public Db CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Db>();
        var cfg = DbConfig.Read();
        cfg.Save();
        string ds = $"Data Source={cfg.Host};port={cfg.Port};Database={cfg.Database};User Id={cfg.Username};password={cfg.Password};charset=utf8;";
        optionsBuilder.UseMySql(ds, ServerVersion.AutoDetect(ds)).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        return new Db(optionsBuilder.Options);
    }
}