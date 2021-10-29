using System.IO;

namespace Module.Util.Extensao
{
    public static class StreamExtension
    {
        public static byte[] ToArray(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.Position = 0;
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;

            return memoryStream.ToArray();
        }
    }
}