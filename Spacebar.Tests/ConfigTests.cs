using System;
using System.Collections;
using System.IO;
using Spacebar.ConfigModel;
using Spacebar.Static.Enums;
using Xunit;
using Xunit.Abstractions;

namespace Spacebar.Tests;

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
        if (File.Exists("config.json"))
            File.Delete("config.json");
        Config.Read("config.json").Save("config.json");
    }

    [Fact]
    public void PermissionSerializerTest()
    {
        var rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();

        var cfg = Config.Instance;
        for (var test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);

            for (var i = 0; i < bits.Count; i++) bits[i] = rnd.Next(0, 2) == 1;
            //output.WriteLine(String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0")));
            cfg.Security.Register.DefaultRights = bits;
            cfg.Save();
            Config.Instance = null!;
            cfg = Config.Instance;
            foreach (var f in fields) Assert.Equal(bits, cfg.Security.Register.DefaultRights);
        }
    }

    [Fact]
    public void ConfigReproducibilityTest()
    {
        var cfg = Config.Instance;
        cfg.Save();
        Assert.True(File.Exists(Config.Path));
        if (File.Exists("config.json.old"))
            File.Delete("config.json.old");
        File.Copy(Config.Path, "config.json.old");
        Config.Instance = null!;
        var cfg2 = Config.Instance;
        cfg2.Save();

        Assert.Equal(File.ReadAllText("config.json.old"), File.ReadAllText(Config.Path));
    }
}