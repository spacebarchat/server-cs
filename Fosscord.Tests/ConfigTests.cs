using System;
using System.Collections;
using System.IO;
using Fosscord.ConfigModel;
using Fosscord.Static.Enums;
using Fosscord.Static.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Fosscord.Tests;

public class ConfigTests
{
    
    private readonly ITestOutputHelper output;

    public ConfigTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void ReadWriteTest()
    {
        if(File.Exists("config.json"))
            File.Delete("config.json");
        Config.Read("config.json").Save("config.json");
    }

    [Fact]
    public void PermissionSerializerTest()
    {
        Random rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        
        var cfg = Config.Read("config.json");
        for (int test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);
            
            for (var i = 0; i < bits.Count; i++)
            {
                bits[i] = rnd.Next(0, 2) == 1;
            }
            //output.WriteLine(String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0")));
            cfg.Security.Register.DefaultRights = bits;
            cfg.Save("config.json");
            cfg = Config.Read("config.json");
            foreach (var f in fields)
            {
                Assert.Equal(bits, cfg.Security.Register.DefaultRights);
            }
        }
    }

    [Fact]
    public void ConfigReproducibilityTest()
    {
        var cfg = Config.Read("config.json");
        cfg.Save("config.json");
        var cfg2 = Config.Read("config.json");
        Assert.Equal(cfg, cfg2);
    }
}