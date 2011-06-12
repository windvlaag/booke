using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Booke.Mobipocket
{
    public class LZ77Decoder
    {
        public int GetByteCount(byte[] bytes, int index, int count)
        {
            int result = 0;

            for (int i = index; i < count + index; i++)
            {
                if (bytes[i] == 0x00)
                    result++;
                else if (bytes[i] >= 0x09 && bytes[i] <= 0x7f)
                    result++;
                else if (bytes[i] >= 0x01 && bytes[i] <= 0x08)
                {

                    result += bytes[i];
                    i += bytes[i];
                }
                else if (bytes[i] >= 0x80 && bytes[i] <= 0xbf && i < count - 1)
                {
                    short offcount = (short)(bytes[i + 1] & 0x07);
                    i++;
                    result += offcount + 3;
                }
                else
                    result += 2;
            }

            return result;
        }

        public int GetBytes(byte[] bytes, int byteIndex, int byteCount, byte[] outputBytes, int outputIndex)
        {
            int index = outputIndex;
            for (int i = byteIndex; i < byteIndex + byteCount; i++)
            {
                if (bytes[i] == 0x00)
                {
                    // Console.Write(" ");
                    outputBytes[index++] = 0x00;
                }
                else if (bytes[i] >= 0x09 && bytes[i] <= 0x7f)
                {
                    // Console.Write((char)bytes[i]);
                    outputBytes[index++] = bytes[i];
                }

                else if (bytes[i] >= 0x01 && bytes[i] <= 0x08)
                {
                    for (int x = 1; x <= bytes[i]; x++)
                    {
                        // Console.Write((char)bytes[i + x]);
                        outputBytes[index++] = bytes[i + x];
                    }

                    i += bytes[i];
                }

                else if (bytes[i] >= 0x80 && bytes[i] <= 0xbf)
                {
                    if (i == byteCount - 1)
                    {
                       // Console.WriteLine("Trailing byte: {0}", bytes[i]);
                    }
                    else
                    {

                        short offset = (short)((0x3F & bytes[i]) * 32 + ((bytes[i + 1] & 0xF8) >> 3));
                        short offcount = (short)(bytes[i + 1] & 0x07);

                        //Logbook.Debug("Offset is {0}, count is {1}, bytes: {2:B2}", offset, offcount +3, new byte[] { bytes[i], bytes[i + 1] });



                        for (int x = 0; x < offcount + 3; x++)
                        {
                            //Console.Write((char)outputBytes[index + x - offset]);
                            outputBytes[index + x] = outputBytes[index + x - offset];
                        }

                        index += offcount + 3;
                        //Console.ReadLine();
                        i += 1;
                    }
                }

                else
                {
                    //Console.Write(" {0}", (char)(bytes[i] ^ 0x80));
                    outputBytes[index++] = (byte)' ';
                    outputBytes[index++] = (byte)(bytes[i] ^ 0x80);
                }


            }

            return index - outputIndex;
        }
    }
}
