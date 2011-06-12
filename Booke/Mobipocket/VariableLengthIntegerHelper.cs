using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    public static class VariableLengthIntegerHelper
    {
        public static int GetBackwardEncodedInteger(ArraySegment<byte> bytes)
        {
            int bytesUsed;
            return GetBackwardEncodedInteger(bytes, out bytesUsed);
        }

        public static int GetBackwardEncodedInteger(ArraySegment<byte> bytes, out int bytesUsed)
        {
            if (bytes.Array == null)
                throw new ArgumentException("bytes array cannot be null");

            bytesUsed = 0;

            checked
            {
                int result = 0;

                for(int index = bytes.Offset + bytes.Count -1; index >=0; index--)
                {
                    var currentByte = bytes.Array[index];

                    bytesUsed++;

                    result <<=7;
                    result += currentByte & 0x7f;

                    if ((currentByte & 0x80) == 0x80)
                        break;
                }

                return result;
            }
        }
    }
}
