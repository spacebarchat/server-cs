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
        optionsBuilder.UseNpgsql(
                $"Host={cfg.Host};Database={cfg.Database};Username={cfg.Username};Password={cfg.Password};Port={cfg.Port};Include Error Detail=true")
            .LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        return new Db(optionsBuilder.Options);
    }
}