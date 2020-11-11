using System.IO;
using System.IO.Compression;
class Compressor
{
    public static byte[] Compress(byte[] a_Data)
    {
        MemoryStream outStream = new MemoryStream();
        using (var compressor = new DeflateStream(outStream, CompressionLevel.Optimal))
        {
            compressor.Write(a_Data, 0, a_Data.Length);
        }

        return outStream.ToArray();
    }

    public static byte[] Decompress(byte[] a_Data)
    {
        MemoryStream inStream = new MemoryStream(a_Data);
        MemoryStream outStream = new MemoryStream();

        using (var decompressor = new DeflateStream(inStream, CompressionMode.Decompress))
        {
            decompressor.CopyTo(outStream);
        }

        return outStream.ToArray();
    }
}