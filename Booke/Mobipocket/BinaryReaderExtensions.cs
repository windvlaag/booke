using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Extension class for binary reader for working with small endian.
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream using provided endian and advances the
        ///  position of the stream by four bytes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="endian">Endian of binary stream.</param>
        /// <returns></returns>
        public static uint ReadUInt32(this BinaryReader reader, Endian endian)
        {
            if(endian == Endian.SmallEndian)
                return reader.ReadUInt32();

            var buffer = new byte[4];
            reader.Read(buffer, 0, 4);

            return BitConverter.ToUInt32(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream  using provided endian and advances the
        ///  position of the stream by four bytes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="endian">Endian of binary stream.</param>
        /// <returns></returns>
        public static int ReadInt32(this BinaryReader reader, Endian endian)
        {
            if (endian == Endian.SmallEndian)
                return reader.ReadInt32();

            var buffer = new byte[4];
            reader.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);
        }


        /// <summary>
        /// Reads a 2-byte signed integer from the current stream using privded endian and advances the current
        /// position of the stream by two bytes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="endian">Endian of binary stream.</param>
        /// <returns></returns>
        public static short ReadInt16(this BinaryReader reader, Endian endian)
        {
            if(endian == Endian.SmallEndian)
                return reader.ReadInt16();

            var buffer = new byte[2];
            reader.Read(buffer, 0, 2);

            return BitConverter.ToInt16(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream using privded endian and advances the current
        /// position of the stream by two bytes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="endian">Endian of binary stream.</param>
        /// <returns></returns>
        public static ushort ReadUInt16(this BinaryReader reader, Endian endian)
        {
            if (endian == Endian.SmallEndian)
                return reader.ReadUInt16();

            var buffer = new byte[2];
            reader.Read(buffer, 0, 2);

            return BitConverter.ToUInt16(buffer.Reverse().ToArray(), 0);
        }
    }
}
