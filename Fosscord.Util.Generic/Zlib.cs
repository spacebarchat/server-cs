using System.IO.Compression;
using System.Text;

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
        Console.WriteLine("Source: " + Encoding.UTF8.GetString(a));
        using var ms = new MemoryStream();
        using var stream = new ZLibStream(ms, CompressionLevel.Optimal);
        var src = new MemoryStream(a);
        src.CopyTo(stream);
        Console.WriteLine("Before flush: " + ms.Length);
        stream.Flush();
        Console.WriteLine("After flush: " + ms.Length);

        var data = ms.ToArray();
        //decompress and log
        var decompressed = Decompress(data);
        Console.WriteLine("Decompressed: " + Encoding.UTF8.GetString(decompressed));
        File.WriteAllBytes("data.bin", data);
        File.WriteAllBytes("datadec.bin", decompressed);
        
        return data;
    }
}