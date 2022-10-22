using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fosscord.Static.Attributes;
using Fosscord.Static.Enums;
using Fosscord.Static.Extensions;
using Fosscord.Static.Utilities;
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
    public void RequireSingleRightTest()
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
    [Fact]
    public void RequireAnyRightTest()
    {
        Random rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (int test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);
            var bits2 = new BitArray(fields.Length, false);
            
            for (var i = 0; i < bits.Count; i++)
            {
                bits[i] = rnd.Next(0, 2) == 1;
                bits2[i] = rnd.Next(0, 2) == 1;
            }

            foreach (var f in fields)
            {
                var right = (int) f.GetValue(null);
                List<int> _bits2 = new List<int>();
                for (int i = 0; i < bits2.Count; i++)
                {
                    if(bits2[i]) _bits2.Add(i);
                }
                var rra = new RequireRightsAttribute(anyRights: _bits2.ToArray());
                var expected = _bits2.Any(x=>bits[x]) || bits[Rights.OPERATOR];
                if(expected != rra.HasRights(bits)) 
                    output.WriteLine($"Expected {expected} but got {rra.HasRights(bits)} for {f.Name} in {String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0"))}");
                Assert.Equal(expected, rra.HasRights(bits));
            }
        }
    }

    public void RequireAllRightsTest()
    {
        Random rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (int test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);
            var bits2 = new BitArray(fields.Length, false);
            
            for (var i = 0; i < bits.Count; i++)
            {
                bits[i] = rnd.Next(0, 2) == 1;
                bits2[i] = rnd.Next(0, 2) == 1;
            }

            foreach (var f in fields)
            {
                var right = (int) f.GetValue(null);
                List<int> _bits2 = new List<int>();
                for (int i = 0; i < bits2.Count; i++)
                {
                    if(bits2[i]) _bits2.Add(i);
                }
                var rra = new RequireRightsAttribute(allRights: _bits2.ToArray());
                var expected = _bits2.All(x=>bits[x]) || bits[Rights.OPERATOR];
                if(expected != rra.HasRights(bits)) 
                    output.WriteLine($"Expected {expected} but got {rra.HasRights(bits)} for {f.Name} in {String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0"))}");
                Assert.Equal(expected, rra.HasRights(bits));
            }
        }
    }

    [Fact]
    public void NullRightsCheck()
    {
        var rra = new RequireRightsAttribute();
        rra.HasRights(new BitArray(32, false));
    }

}