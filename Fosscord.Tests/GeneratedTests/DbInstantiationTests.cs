using Fosscord.DbModel;
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
    public void GetNewMysqlTest()
    {
        var db = Db.GetNewMysql(DbConfig.FromEnv("GetNewMysql".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        output.WriteLine(db.GetType().ToString());
    }
    
    [Fact]
    public void GetNewPostgresTest()
    {
        var db = Db.GetNewPostgres(DbConfig.FromEnv("GetNewPostgres".Replace("GetNew","Get").Replace("Get","FC_CS_UNIT_TEST_").ToUpper()));
        output.WriteLine(db.GetType().ToString());
    }
    

}