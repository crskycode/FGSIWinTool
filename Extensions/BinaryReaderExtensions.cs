using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FGSIWinTool.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var bytes = new List<byte>(1024);

            for (var c = reader.ReadByte(); c != 0; c = reader.ReadByte())
            {
                bytes.Add(c);
            }

            if (bytes.Count > 0)
            {
                return Encoding.UTF8.GetString(bytes.ToArray());
            }

            return string.Empty;
        }

        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            return BinaryPrimitives.ReverseEndianness(reader.ReadInt32());
        }
    }
}
