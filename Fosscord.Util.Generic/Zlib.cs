using System.IO.Compression;

namespace Fosscord.Util;

public class ZLib
{
    public static byte[] Decompress(byte[] a)
    {
        var sss = new MemoryStream(a);

        MemoryStream decodedStream = new();
        byte[] buffer = new byte[4096];

        using (DeflateStream c = new(sss, CompressionMode.Decompress))
        {
            int bytesRead;
            while ((bytesRead = c.Read(buffer, 0, buffer.Length)) > 0)
                decodedStream.Write(buffer, 0, bytesRead);
        }

        return decodedStream.ToArray();
    }

    public static byte[] Compress(byte[] a)
    {
        MemoryStream input = new(a);

        MemoryStream compressStream = new();

        using DeflateStream compressor = new(compressStream, CompressionLevel.NoCompression, true);
        input.CopyTo(compressor);
        compressor.Close();

        //compressStream.Write(new byte[] { 0, 0, 0xFF, 0xFF });
        var ar = compressStream.ToArray();

        //TODO: remove this hack
        //ar[0] = (byte)(ar[0] - 1); //before: 0xab
        return ar;
    }
}