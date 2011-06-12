using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    public class TrailingEntryParser
    {
        private uint _trailingEntriesFlags;

        public TrailingEntryParser(ushort trailingEntryFlag)
        {
            this._trailingEntriesFlags = trailingEntryFlag;
        }


        /// <summary>
        /// gets list of bit flags from trailing entry variable.
        /// </summary>
        /// <param name="trailingEntriesFlags"></param>
        /// <returns></returns>
        public IEnumerable<ushort> Flags
        {
            get
            {
                ushort value = 0x8000;

                while (value > 0)
                {
                    if ((_trailingEntriesFlags & value) == value)
                        yield return value;

                    value >>= 1;
                }
            }
        }

        /// <summary>
        /// Returns a collection of trailing entry data from binary
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public TrailingEntryCollection Parse(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            try
            {
                //create entry collection
                var entries = new TrailingEntryCollection();

                //take array segment with all bytes
                var segment = new ArraySegment<byte>(bytes);

                foreach (var flag in Flags)
                {

                    if (flag != TrailingEntry.MultibyteCharacterOverlapFlag)
                        //add simple entry
                        entries.AddEntry(ExtractTrailingEntry(ref segment, flag));

                    else 
                        //add overlap entry
                        entries.MultibyteCharacterOverlap = ExtractCharacterOverlap(ref segment);
                }

                //adjust byte count
                entries.ByteCount = bytes.Length - segment.Count;

                //return result
                return entries;
            }
            catch (Exception e)
            {
                throw new Exception("cannot parse trailing entries", e);
            }
        }



        //---------------------------------------------
        //
        // Private methods
        //
        //---------------------------------------------

        #region Private methods

        /// <summary>
        /// Extracts last trailing entry from data and resizes data left
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        private TrailingEntry ExtractTrailingEntry(ref ArraySegment<byte> segment, ushort flag)
        {
            if (segment.Array == null)
                throw new ArgumentException("segment bytes cannot be null");

            if (flag == TrailingEntry.MultibyteCharacterOverlapFlag)
                throw new ArgumentException("trailing entry 0x01 not supported");

            //bytes used for encoding size
            int bytesUsed;

            //get trailing entry size
            int trailingEntrySize = VariableLengthIntegerHelper.GetBackwardEncodedInteger(segment, out bytesUsed);

            //buffer for entry data
            byte[] buffer = new byte[ trailingEntrySize - bytesUsed ];

            //copy data
            Array.Copy(segment.Array, segment.Offset + segment.Count - trailingEntrySize, buffer, 0, trailingEntrySize - bytesUsed);

            //adjust segment
            segment = new ArraySegment<byte>(segment.Array, segment.Offset, segment.Count - trailingEntrySize);

            //return result
            return new TrailingEntry { Flag = flag, Data = buffer };
        }

        private byte[] ExtractCharacterOverlap(ref ArraySegment<byte> segment)
        {
            if (segment.Array == null)
                throw new ArgumentException("segment bytes cannot be null");

            //get overlap size
            int byteCount = segment.Array[segment.Count - 1] & 0x03;

            //buffer for overlap data
            var buffer = new byte[byteCount];

            if (byteCount > 0)
            {
                //copy data
                Array.Copy(segment.Array, segment.Offset + segment.Count - byteCount - 1, buffer, 0, byteCount);
            }

            //adjust segment
            segment = new ArraySegment<byte>(segment.Array, segment.Offset, segment.Count - byteCount - 1);

            //return result
            return buffer;
        }

        private static byte[] GetMultibateCharacterOverlap(byte[] bytes, ref int negativeOffset)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            //adjust negative offset
            var sizeByte = bytes[bytes.Length - ++negativeOffset];

            //only two most significant bits make up byte count
            int byteCount = (sizeByte & 3);

            if (byteCount > 0)
            {
                //allocate memory
                byte[] output = new byte[byteCount];

                //copy data
                Array.Copy(bytes, bytes.Length - negativeOffset - byteCount, output, 0, byteCount);

                //adjust offset
                negativeOffset += byteCount;

                //return result
                return output;
            }

            //no overlap
            return null;
        }


        #endregion
    }
}
