using System;
using System.IO;
using Spacebar.Util;
using Xunit;
using Xunit.Abstractions;

namespace Spacebar.Tests;

public class ZlibTests
{
    private readonly ITestOutputHelper output;

    public ZlibTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    //these represent the Heartbeat_ACK data
    private byte[] knownCompressed =
    {
        0x78, 0x9c, 0xab, 0xe6, 0x52, 0x50, 0x50, 0xca, 0x2f, 0x50, 0xb2, 0x52, 0x30, 0x34, 0xe4, 0xaa, 0x05, 0x00,
        0x17, 0xb7, 0x03, 0x2c
    };

    private byte[] knownDecompressed =
        { 0x7b, 0x0a, 0x20, 0x20, 0x22, 0x6f, 0x70, 0x22, 0x3a, 0x20, 0x31, 0x31, 0x0a, 0x7d };

    [Fact]
    public byte[] Decompress()
    {
        var data = ZLib.Decompress(knownCompressed);
        Assert.Equal(knownDecompressed, data);
        return data;
    }

    [Fact]
    public byte[] Compress()
    {
        var data = ZLib.Compress(knownDecompressed);
        if (knownCompressed != data)
        {
            output.WriteLine("Expected: " + BitConverter.ToString(knownCompressed).Replace("-", " "));
            output.WriteLine("Actual:   " + BitConverter.ToString(data).Replace("-", " "));
        }

        Assert.Equal(knownCompressed, data);
        return data;
    }

    [Fact]
    public byte[] Compare()
    {
        var data = ZLib.Compress(knownDecompressed);
        var data2 = ZLib.Decompress(data);
        Assert.Equal(knownDecompressed, data2);
        return data;
    }

    [Fact]
    public byte[] CompressIonic()
    {
        var zs = new Ionic.Zlib.ZlibStream(new MemoryStream(knownDecompressed), Ionic.Zlib.CompressionMode.Compress,
            Ionic.Zlib.CompressionLevel.Default);
        var ms = new MemoryStream();
        zs.CopyTo(ms);

        var data = ms.ToArray();
        if (knownCompressed != data)
        {
            output.WriteLine("Expected: " + BitConverter.ToString(knownCompressed).Replace("-", " "));
            output.WriteLine("Actual:   " + BitConverter.ToString(data).Replace("-", " "));
        }

        Assert.Equal(knownCompressed, data);
        return data;
    }
}