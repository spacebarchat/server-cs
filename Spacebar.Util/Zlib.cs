using System.IO.Compression;
using Ionic.Zlib;
using CompressionLevel = Ionic.Zlib.CompressionLevel;
using CompressionMode = System.IO.Compression.CompressionMode;

namespace Spacebar.Util;

public class ZLib {
    public static byte[] Decompress(byte[] a) {
        var src = new MemoryStream(a);
        var dst = new MemoryStream();
        using var stream = new ZLibStream(src, CompressionMode.Decompress);
        stream.CopyTo(dst);
        stream.Flush();
        return dst.ToArray();
    }

    public static byte[] Compress(byte[] a) {
        ZlibStream zs = new(new MemoryStream(a), Ionic.Zlib.CompressionMode.Compress,
            CompressionLevel.Default);
        var ms = new MemoryStream();
        zs.CopyTo(ms);
        var data = ms.ToArray();
        return data;
    }
}