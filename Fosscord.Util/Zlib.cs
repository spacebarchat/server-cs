using System.IO.Compression;

namespace Fosscord.Util;

public class ZLib
{
    public static byte[] Decompress(byte[] a)
    {
        var src = new MemoryStream(a);
        var dst = new MemoryStream();
        using var stream = new ZLibStream(src, CompressionMode.Decompress);
        stream.CopyTo(dst);
        stream.Flush();
        return dst.ToArray();
    }

    public static byte[] Compress(byte[] a)
    {
        Ionic.Zlib.ZlibStream zs = new(new MemoryStream(a), Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.Default);
        var ms = new MemoryStream();
        zs.CopyTo(ms);
        var data = ms.ToArray();
        return data;
    }
}