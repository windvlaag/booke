using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Represents record in mobipocket document.
    /// </summary>
    public class MobipocketRecord
    {
        /// <summary>
        /// Creates new mobipocket record from byte data
        /// </summary>
        /// <param name="data"></param>
        public MobipocketRecord()
        {

        }

        public static MobipocketRecord FromBinary(byte[] data, bool isTextRecord, TrailingEntryParser parser)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (isTextRecord && parser == null)
                throw new ArgumentException("parser cannot be null if isTextRecord is true");

            var record = new MobipocketRecord { Data = data, IsTextRecord = isTextRecord };

            if (isTextRecord)
            {
                try
                {
                    record.TrailingEntries = parser.Parse(data);
                }
                catch (Exception e)
                {
                    throw new Exception("cannot parse trailing entries for record", e);
                }
            }

            return record;
        }


        public static MobipocketRecord FromBinary(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return new MobipocketRecord { Data = data, IsTextRecord = false };
        }


        /// <summary>
        /// Gets or sets original data of record.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Whether record is text record
        /// </summary>
        public bool IsTextRecord { get;set; }

        /// <summary>
        /// Gets or sets trailing entries associated with record.
        /// </summary>
        /// <remarks>
        /// This property is only set if IsTextRecord equals true.
        /// </remarks>
        public TrailingEntryCollection TrailingEntries { get; set; }

        /// <summary>
        /// Gets or sets text associated with record.
        /// </summary>
        /// <remarks>
        /// This property is only set if IsTextRecord equals true.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// FLIS record
        /// </summary>
        public static readonly MobipocketRecord FLIS = MobipocketRecord.FromBinary(new byte[] { 
            0x46, 0x4c, 0x49, 0x53, 0x00, 0x00, 0x00, 0x08,
            0x00, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xff, 0xff, 0xff, 0xff, 0x00, 0x01, 0x00, 0x03,
            0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01,
            0xff, 0xff, 0xff, 0xff
            });

        /// <summary>
        /// FCIS record
        /// </summary>
        public static readonly MobipocketRecord FCIS = MobipocketRecord.FromBinary(new byte[] { 
            0x46, 0x43, 0x49, 0x53, 0x00, 0x00, 0x00, 0x14,
            0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20,
            0x00, 0x00, 0x00, 0x08, 0x00, 0x01, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x00
            });

        /// <summary>
        /// EOF record
        /// </summary>
        public static readonly MobipocketRecord EOF = MobipocketRecord.FromBinary(new byte[]{ 
            0xe9, 0x8e, 0x0d, 0x0a
            });

        public static bool operator==(MobipocketRecord a, MobipocketRecord b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            if( a.Data != null && b.Data != null)
            {
                if(a.Data.Length == b.Data.Length)
                {
                    for(int index = 0; index < a.Data.Length; index++)
                    {
                        if(a.Data[index] != b.Data[index])
                            return false;
                    }

                    return a.IsTextRecord == b.IsTextRecord;
                }
            }

            return false;
        }

        public static bool operator !=(MobipocketRecord a, MobipocketRecord b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is MobipocketRecord)
                return this == obj as MobipocketRecord;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public ArraySegment<byte> GetDataWithoutTrailingEntries()
        {
            return new ArraySegment<byte>(Data, 0, Data.Length - TrailingEntries.ByteCount);
        }

        public bool IsFLISRecord 
        {
            get
            {
                if (!IsTextRecord)
                    if (Data[0] == FLIS.Data[0] &&
                        Data[1] == FLIS.Data[1] &&
                        Data[2] == FLIS.Data[2] &&
                        Data[3] == FLIS.Data[3])
                        return true;

                return false;
            }
        }

        public bool IsFCISRecord
        {
            get
            {
                if (!IsTextRecord)
                    if (!IsTextRecord)
                        if (Data[0] == FCIS.Data[0] &&
                            Data[1] == FCIS.Data[1] &&
                            Data[2] == FCIS.Data[2] &&
                            Data[3] == FCIS.Data[3])
                            return true;
                return false;
            }
        }

        public bool IsEOFRecord
        {
            get
            {
                if (!IsTextRecord)
                    if (Data[0] == EOF.Data[0] &&
                        Data[1] == EOF.Data[1] &&
                        Data[2] == EOF.Data[2] &&
                        Data[3] == EOF.Data[3])
                        return true;

                return false;
            }
        }
    }
}
