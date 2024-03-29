using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spacebar.Static.Attributes;
using Spacebar.Static.Enums;
using Spacebar.Static.Extensions;
using Spacebar.Static.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Spacebar.Tests;

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
        var rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (var test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);

            for (var i = 0; i < bits.Count; i++) bits[i] = rnd.Next(0, 2) == 1;
            //output.WriteLine(String.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0")));

            foreach (var f in fields) Assert.Equal(bits[(int)f.GetValue(null)], bits.HasFlag((int)f.GetValue(null)));
        }
    }

    [Fact]
    public void RandomStringGeneratorMakesRandomStrings()
    {
        var strings = new HashSet<string>();
        for (var i = 0; i < 1000; i++)
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
        var rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (var test = 0; test < 1000; test++)
        {
            var bits = new BitArray(fields.Length, false);

            for (var i = 0; i < bits.Count; i++) bits[i] = rnd.Next(0, 2) == 1;

            foreach (var f in fields)
            {
                var right = (int)f.GetValue(null);
                var rra = new RequireRightsAttribute(right);
                var expected = bits[right] || bits[Rights.OPERATOR];
                if (expected != rra.HasRights(bits))
                    output.WriteLine(
                        $"Expected {expected} but got {rra.HasRights(bits)} for {f.Name} in {string.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0"))}");
                Assert.Equal(expected, rra.HasRights(bits));
            }
        }
    }

    [Fact]
    public void RequireAnyRightTest()
    {
        var rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (var test = 0; test < 1000; test++)
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
                var right = (int)f.GetValue(null);
                var _bits2 = new List<int>();
                for (var i = 0; i < bits2.Count; i++)
                    if (bits2[i])
                        _bits2.Add(i);
                var rra = new RequireRightsAttribute(anyRights: _bits2.ToArray());
                var expected = _bits2.Any(x => bits[x]) || bits[Rights.OPERATOR];
                if (expected != rra.HasRights(bits))
                    output.WriteLine(
                        $"Expected {expected} but got {rra.HasRights(bits)} for {f.Name} in {string.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0"))}");
                Assert.Equal(expected, rra.HasRights(bits));
            }
        }
    }

    public void RequireAllRightsTest()
    {
        var rnd = new Random();
        var t = typeof(Rights);
        var fields = t.GetFields();
        for (var test = 0; test < 1000; test++)
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
                var right = (int)f.GetValue(null);
                var _bits2 = new List<int>();
                for (var i = 0; i < bits2.Count; i++)
                    if (bits2[i])
                        _bits2.Add(i);
                var rra = new RequireRightsAttribute(allRights: _bits2.ToArray());
                var expected = _bits2.All(x => bits[x]) || bits[Rights.OPERATOR];
                if (expected != rra.HasRights(bits))
                    output.WriteLine(
                        $"Expected {expected} but got {rra.HasRights(bits)} for {f.Name} in {string.Join("", bits.Cast<bool>().Select(b => b ? "1" : "0"))}");
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