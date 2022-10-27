using System;
using System.Linq;
using System.Reflection;
using AngleSharp.Text;
using Fosscord.DbModel;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Fosscord.Tests;

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
        var db = Db.GetDb();
        output.WriteLine(db.GetType().ToString());
    }

    [Fact]
    public void MigAsmDetectionFinder()
    {
        var tests = "postgres,mysql,mariadb,sqlite,inmemory,test".SplitCommas();
        foreach (var test in tests)
        {
            output.WriteLine($"{test} -> {Db.GetMigAsm(test)}");
        }
    }

    [Fact]
    public void EnumerateDbSets()
    {
        var db = Db.GetInMemoryDb();
        var dbType = db.GetType();
        var props = dbType.GetProperties().Where(x=>x.PropertyType.Name.Contains("DbSet")).ToList();
        foreach (var propertyInfo in props)
        {
            var dbSet = (DbSet<object>) propertyInfo.GetValue(db);
            if (dbSet == null) continue;
            var ent = Activator.CreateInstance(dbSet.EntityType.ClrType);
            var entType = ent.GetType();
            
        }
    }
}