using Spacebar.DbModel;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Spacebar.Tests;

public class DbTests
{
    private readonly ITestOutputHelper output;

    public DbTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Test1()
    {
        try
        {
            var db = Db.GetDb();
            db.Database.EnsureDeleted();
            db.Dispose();
            db = Db.GetNewDb();
            db.Database.Migrate();
            db.Dispose();
        }
        catch
        {
        }
    }

    [Fact]
    public void MigAsmDetectionFinder()
    {
        var tests = "postgres,mysql,mariadb,sqlite,inmemory,test".Split(',');
        foreach (var test in tests) output.WriteLine($"{test} -> {Db.GetMigAsm(test)}");
    }

    /*[Fact]
    public void EnumerateDbSets()
    {
        var db = Db.GetInMemoryDb();
        var dbType = db.GetType();
        var props = dbType.GetProperties().Where(x => x.PropertyType.Name.Contains("DbSet")).ToList();
        foreach (var propertyInfo in props)
        {
            var dbSet = (DbSet<object>) 
                propertyInfo.GetMethod.Invoke(db, null);
            dbSet.ToList();
            if (dbSet == null) continue;
            var ent = Activator.CreateInstance(dbSet.EntityType.ClrType);
            var entType = ent.GetType();
        }
    }*/
}