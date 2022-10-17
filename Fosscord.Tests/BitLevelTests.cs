using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fosscord.API.Utilities;
using Fosscord.Shared.Attributes;
using Fosscord.Shared.Enums;
using Fosscord.Util.Generic.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Fosscord.Tests;

public class BitLevelTests
{
    private readonly ITestOutputHelper output;

    public BitLevelTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void HasFlagTest()
    {
        Random rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (int test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);
            
            for (var i = 0; i < bits.Count; i++)
            {
                bits[i] = rnd.Next(0, 2) == 1;
            }
            //output.WriteLine(String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0")));

            foreach (var f in fields)
            {
                Assert.Equal(bits[(int) f.GetValue(null)], bits.HasFlag((int) f.GetValue(null)));
            }
        }
    }
    [Fact]
    public void RandomStringGeneratorMakesRandomStrings()
    {
        var strings = new HashSet<string>();
        for (int i = 0; i < 1000; i++)
        {
            var s = RandomStringGenerator.Generate(10);
            Assert.DoesNotContain(s, strings);
            strings.Add(s);
            //output.WriteLine(s);
        }
        Assert.Equal(1000, strings.Count);
    }

    [Fact]
    public void RequireRightsAttributeTest()
    {
        Random rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (int test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);
            
            for (var i = 0; i < bits.Count; i++)
            {
                bits[i] = rnd.Next(0, 2) == 1;
            }

            foreach (var f in fields)
            {
                var right = (int) f.GetValue(null);
                var rra = new RequireRightsAttribute(right);
                var expected = bits[right] || bits[Rights.OPERATOR];
                if(expected != rra.HasRights(bits)) 
                    output.WriteLine($"Expected {expected} but got {rra.HasRights(bits)} for {f.Name} in {String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0"))}");
                Assert.Equal(expected, rra.HasRights(bits));
            }
        }
    }
}