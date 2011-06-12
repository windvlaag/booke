using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke.Mobipocket.PalmDatabase
{
    internal static class BigEndianBitConverter
    {
        internal static uint ReadUInt32(this BinaryReader reader)
        {
            return 0;
        }

        public static uint ToUInt32(byte[] value, int startIndex)
        {
            if (startIndex + 4 > value.Length)
                throw new ArgumentOutOfRangeException("startIndex");

            return BitConverter.ToUInt32(new byte[] { 
                value[startIndex+3], value[startIndex+2], value[startIndex+1], value[startIndex] }, 0);
        }

        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            if (startIndex + 2 > value.Length)
                throw new ArgumentOutOfRangeException("startIndex");

            return BitConverter.ToUInt16(new byte[] { value[startIndex + 1], value[startIndex] }, 0);       
        }


    }
}
