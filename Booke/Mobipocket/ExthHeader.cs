using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke.Mobipocket
{
    public class ExthHeader
    {
        //---------------------------------------------
        //
        // Fields
        //
        //---------------------------------------------

        #region Fields

        // 'EXTH' in hex
        private const uint ExthIdentifier = 0x45585448;
        private const int MinimalHeaderSize = 12; 
        private uint _length;

        #endregion

        //---------------------------------------------
        //
        // .ctors
        //
        //---------------------------------------------

        #region .ctors

        public ExthHeader()
        {
            Records = new List<ExthRecord>();
        }

        public static ExthHeader FromBinary(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            return FromBinary(new ArraySegment<byte>(bytes));
        }

        public static ExthHeader FromBinary(ArraySegment<byte> bytes)
        {
            if(bytes.Array == null)
                throw new ArgumentException("bytes array cannot be null");

            var header = new ExthHeader();

            try
            {
                if (bytes.Count < MinimalHeaderSize)
                    throw new Exception("bytes not long enough");

                using(var stream = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count))
                using (var reader = new BinaryReader(stream))
                {
                    ////seek in stream
                    //reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                    //grab identifier
                    var identifier = reader.ReadUInt32(Endian.BigEndian);
                    if (ExthIdentifier != identifier)
                        throw new NotSupportedException("identifier not supported");

                    //grab header length
                    header._length = reader.ReadUInt32(Endian.BigEndian);

                    //grab record count
                    var recordCount = reader.ReadUInt32(Endian.BigEndian);

                    //grab records
                    for (int recordIndex = 0; recordIndex < recordCount; recordIndex++)
                    {
                        uint recordType = reader.ReadUInt32(Endian.BigEndian);

                        //substract 8 for 8 bytes alread red
                        uint recordDataLength = reader.ReadUInt32(Endian.BigEndian) - 8;

                        byte[] recordData = new byte[recordDataLength];

                        checked { reader.Read(recordData, 0, (int)recordDataLength); }

                        header.Records.Add(new ExthRecord(recordType, recordData));
                    }

                    //additional zero padding
                }

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Exception caught when parsing exth header", e);
            }
        } 

        #endregion

        //---------------------------------------------
        //
        // Properties
        //
        //---------------------------------------------

        #region Properties

        public List<ExthRecord> Records { get; set; }

        #endregion

    }
}
