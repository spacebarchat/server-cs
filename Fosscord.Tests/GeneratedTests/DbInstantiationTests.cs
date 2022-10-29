using Fosscord.DbModel;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Fosscord.Tests;

public class DbInstantiationTests
{
    private readonly ITestOutputHelper output;

    public DbInstantiationTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    [Fact]
    public void GetNewDbTest()
    {
        try {
            var db = Db.GetNewDb(DbConfig.FromEnv("GetNewDb".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        }
        catch (Exception e)
        {
            output.WriteLine("GetNewDb: Could not connect...");
            if (new[] {"MySqlException", "NpgsqlException"}.Contains(e.GetType().Name)) return;
            if (e.Message.Contains("does not exist")) return;
            throw;
        }
    }
    
    [Fact]
    public void GetNewMysqlTest()
    {
        try {
            var db = Db.GetNewMysql(DbConfig.FromEnv("GetNewMysql".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        }
        catch (Exception e)
        {
            output.WriteLine("GetNewMysql: Could not connect...");
            if (new[] {"MySqlException", "NpgsqlException"}.Contains(e.GetType().Name)) return;
            if (e.Message.Contains("does not exist")) return;
            throw;
        }
    }
    
    [Fact]
    public void GetNewPostgresTest()
    {
        try {
            var db = Db.GetNewPostgres(DbConfig.FromEnv("GetNewPostgres".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        }
        catch (Exception e)
        {
            output.WriteLine("GetNewPostgres: Could not connect...");
            if (new[] {"MySqlException", "NpgsqlException"}.Contains(e.GetType().Name)) return;
            if (e.Message.Contains("does not exist")) return;
            throw;
        }
    }
    
    [Fact]
    public void GetSqliteTest()
    {
        try {
            var db = Db.GetSqlite(DbConfig.FromEnv("GetSqlite".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        }
        catch (Exception e)
        {
            output.WriteLine("GetSqlite: Could not connect...");
            if (new[] {"MySqlException", "NpgsqlException"}.Contains(e.GetType().Name)) return;
            if (e.Message.Contains("does not exist")) return;
            throw;
        }
    }
    
    [Fact]
    public void GetInMemoryDbTest()
    {
        try {
            var db = Db.GetInMemoryDb(DbConfig.FromEnv("GetInMemoryDb".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        }
        catch (Exception e)
        {
            output.WriteLine("GetInMemoryDb: Could not connect...");
            if (new[] {"MySqlException", "NpgsqlException"}.Contains(e.GetType().Name)) return;
            if (e.Message.Contains("does not exist")) return;
            throw;
        }
    }
    

}