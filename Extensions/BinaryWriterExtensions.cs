using System.Buffers.Binary;
using System.IO;

namespace FGSIWinTool.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void WriteInt32BigEndian(this BinaryWriter writer, int value)
        {
            writer.Write(BinaryPrimitives.ReverseEndianness(value));
        }
    }
}
