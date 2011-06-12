using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke.Mobipocket.PalmDatabase
{
    internal class PalmDatabaseFormat
    {
        private int _appInfoOffset;
        private int _sortInfoOffset;
        private int _uniqueIdSeed;
        private int _nextRecordListID;
        private readonly static Encoding Latin1 = Encoding.GetEncoding("Latin1");

        public PalmDatabaseFormat()
        {
            Records = new List<PalmDatabaseRecord>();
        }


        /// <summary>
        /// Gets or sets palm database name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets palm attributes.
        /// </summary>
        public PalmDatabaseAttributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets file version.
        /// </summary>
        public short Version { get; set; }

        /// <summary>
        /// Gets or sets creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets modification date.
        /// </summary>
        public DateTime ModificationDate { get; set; }

        /// <summary>
        /// Gets or sets last backup date.
        /// </summary>
        public DateTime LastBackupDate { get; set; }

        /// <summary>
        /// Gets or sets modification number.
        /// </summary>
        public int ModificationNumber { get; set; }

        /// <summary>
        /// Gets or sets type of database.
        /// </summary>
        /// <remarks>
        /// This needs to be exactly 4 characters long. 
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets creator.
        /// </summary>
        /// <remarks>
        /// This need to be exactly 4 characters long.
        /// </remarks>
        public string Creator { get; set; }

        /// <summary>
        /// Gets list of records.
        /// </summary>
        public List<PalmDatabaseRecord> Records { get; private set; }

        public static PalmDatabaseFormat FromBinary(BinaryReader reader)
        {
            try
            {
                //create new palm database
                var palm = new PalmDatabaseFormat();

                //some buffer for reading data
                byte[] buffer = new byte[4096];

                // Read database name
                reader.Read(buffer, 0, 32);
                palm.Name = Latin1.GetString(buffer.TakeWhile(data => !data.Equals(0)).ToArray());

                // Read attributes
                palm.Attributes = (PalmDatabaseAttributes) Enum.ToObject(typeof(PalmDatabaseAttributes), reader.ReadInt16(Endian.BigEndian));

                // Read version
                palm.Version = reader.ReadInt16(Endian.BigEndian);

                // Created time
                palm.CreationDate = DateTimeConverter.GetPDBTimeFromTimestamp(reader.ReadUInt32(Endian.BigEndian));

                // Modified time
                palm.ModificationDate = DateTimeConverter.GetPDBTimeFromTimestamp(reader.ReadUInt32(Endian.BigEndian));

                // Last backup time
                palm.LastBackupDate = DateTimeConverter.GetPDBTimeFromTimestamp(reader.ReadUInt32(Endian.BigEndian));

                // Modification number
                palm.ModificationNumber = reader.ReadInt32(Endian.BigEndian);

                // Internal offsets
                palm._appInfoOffset = reader.ReadInt32(Endian.BigEndian);
                palm._sortInfoOffset = reader.ReadInt32(Endian.BigEndian);

                // read type and creator
                reader.Read(buffer, 0, 8);
                palm.Type = Latin1.GetString(buffer, 0, 4);
                palm.Creator = Latin1.GetString(buffer, 4, 4);

                // More internal data
                palm._uniqueIdSeed = reader.ReadInt32(Endian.BigEndian);
                palm._nextRecordListID = reader.ReadInt32(Endian.BigEndian);

                // Read number of records
                ushort numberOfRecords = reader.ReadUInt16(Endian.BigEndian);

                for (ushort i = 0; i < numberOfRecords; i++)
                {
                    //create new record
                    var record = new PalmDatabaseRecord();
                
                    //read offset of data
                    record.Offset = reader.ReadUInt32(Endian.BigEndian);

                    var category = reader.ReadByte();

                    record.Attributes = (PalmRecordAttributes) Enum.ToObject(typeof(PalmRecordAttributes), category >> 4);
                    record.Category = category & 0xf;

                    // uid
                    reader.Read(buffer, 0, 3);
                    record.Id = BitConverter.ToUInt32(new byte[] {  buffer[2], buffer[1], buffer[0], 0 }, 0);

                    // adjust size of previous record
                    var last = palm.Records.LastOrDefault();

                    if (last != null)
                        last.Size = record.Offset - last.Offset;

                    // add record
                    palm.Records.Add(record);
                }

                // set size of last record
                var lastRecord = palm.Records.LastOrDefault();

                if (lastRecord != null)
                    lastRecord.Size = (uint)reader.BaseStream.Length - lastRecord.Offset;

                foreach (var record in palm.Records)
                {
                    reader.BaseStream.Seek(record.Offset, SeekOrigin.Begin);

                    record.Data = new byte[record.Size];

                    reader.Read(record.Data, 0, (int)record.Size);
                }

                //return the result
                return palm;
            }
            catch (Exception e)
            {
                throw new Exception("cannot read palm database from binary reader", e);
            }
        }

        public static PalmDatabaseFormat FromBinary(ArraySegment<byte> bytes)
        {
            if (bytes.Array == null)
                throw new ArgumentException("bytes cannot be null");

            using(var stream = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count))
            using(var reader = new BinaryReader(stream))
            {
                return FromBinary(reader);
            }
        }

        public byte[] ToBinary()
        {
            List<byte> output = new List<byte>();

            throw new NotImplementedException();
        }
    }
}
