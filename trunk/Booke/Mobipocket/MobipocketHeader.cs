using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Booke.Mobipocket.PalmDatabase;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Represents header od mobipocket document
    /// </summary>
    public class MobipocketHeader
    {
        //---------------------------------------------
        //
        // Fields
        //
        //---------------------------------------------

        #region Fields

        // "MOBI" in hex
        private const uint MobiIdentifier = 0x4d4f4249;

        private uint _fullNameLength;
        private uint _fullNameOffset;
        private bool _exthEnabled;

        #endregion

        //---------------------------------------------
        //
        // .ctor
        //
        //---------------------------------------------

        #region .ctor

        /// <summary>
        /// Creates new instance of mobipocket header
        /// </summary>
        public MobipocketHeader()
        {
            DictionaryInfo = new MobipocketDictionaryInfo();
            HuffmanInfo = new MobipocketHuffmanInfo();
        }

        /// <summary>
        /// Crates new instance of mobipocket header from provided palm database.
        /// </summary>
        /// <param name="pdb"></param>
        /// <returns></returns>
        internal static MobipocketHeader FromPalmDatabase(PalmDatabaseFormat pdb)
        {
            if (pdb == null)
                throw new ArgumentNullException("pdb");

            if (pdb.Records.Count < 1)
                throw new ArgumentException("no records in palm database");

            var bytes =  pdb.Records.First().Data;

            if (bytes == null)
                throw new ArgumentException("first record in palm database is malformed");

            return FromBinary(new ArraySegment<byte>(bytes, 0 ,bytes.Length));
        }

        /// <summary>
        /// Creates new instance of mobipocket header from provided bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static MobipocketHeader FromBinary(ArraySegment<byte> bytes)
        {
            if (bytes.Array == null)
                throw new ArgumentException("bytes array cannot be null");

            var header = new MobipocketHeader();

            try
            {
                using (var stream = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count))
                using (var reader = new BinaryReader(stream))
                {
                    if (bytes.Count < 0x80)
                        throw new Exception("header is to small");

                    //palmdoc header start

                    //Get compression
                    header.Compression = (MobipocketCompression)Enum.ToObject(typeof(MobipocketCompression), reader.ReadUInt16(Endian.BigEndian));

                    //usnused zeroes
                    reader.BaseStream.Seek(2, SeekOrigin.Current);

                    //length of text
                    header.TotalTextLength = reader.ReadUInt32(Endian.BigEndian);

                    //count of dpb records that contain text
                    header.TextRecordCount = reader.ReadUInt16(Endian.BigEndian);

                    //maximum record size, always 4096
                    header.MaxRecordSize = reader.ReadUInt16(Endian.BigEndian);

                    //read mobipocket encryption
                    header.Encryption = (MobipocketEncryption)Enum.ToObject(typeof(MobipocketEncryption), reader.ReadUInt16(Endian.BigEndian));

                    //skip two zeroes
                    reader.BaseStream.Seek(2, SeekOrigin.Current);

                    //palmdoc header end

                    //mobi header start

                    //read identifier "MOBI"
                    var identifier = reader.ReadUInt32(Endian.BigEndian);
                    if (MobiIdentifier != identifier)
                        throw new Exception("unsupported identifier: " + identifier);

                    //read byte length of header
                    var length = reader.ReadUInt32(Endian.BigEndian);

                    //read the type of document
                    header.Type = (MobipocketType)Enum.ToObject(typeof(MobipocketType), reader.ReadUInt32(Endian.BigEndian));

                    //read encoding of document
                    header.Encoding = (MobipocketEncoding)Enum.ToObject(typeof(MobipocketEncoding), reader.ReadUInt32(Endian.BigEndian));

                    //read uid
                    header.UniqueId = reader.ReadUInt32(Endian.BigEndian);

                    //read version
                    header.Version = reader.ReadUInt32(Endian.BigEndian);

                    //40 0xff bytes
                    reader.BaseStream.Seek(40, SeekOrigin.Current);

                    //first no book record
                    header.FirstNonTextRecord = reader.ReadUInt32(Endian.BigEndian);

                    //offset in header of full name data
                    header._fullNameOffset = reader.ReadUInt32(Endian.BigEndian);

                    //length of full name data
                    header._fullNameLength = reader.ReadUInt32(Endian.BigEndian);

                    //locale of mobipocket file
                    header.Locale = reader.ReadUInt32(Endian.BigEndian);

                    //dictionary input language
                    header.DictionaryInfo.InputLanguage = reader.ReadUInt32(Endian.BigEndian);

                    //dictionary output language
                    header.DictionaryInfo.OutputLanguage = reader.ReadUInt32(Endian.BigEndian);

                    //minimal mobi pocket reader version
                    header.MinReaderVersion = reader.ReadUInt32(Endian.BigEndian);

                    //index of first image in pdb database
                    header.FirtImageRecord = reader.ReadUInt32(Endian.BigEndian);

                    //offset of huffman data
                    header.HuffmanInfo.RecordOffset = reader.ReadUInt32(Endian.BigEndian);

                    //count of huffman data
                    header.HuffmanInfo.RecordCount = reader.ReadUInt32(Endian.BigEndian);

                    //Skip 8 bytes, often zeroes
                    reader.BaseStream.Seek(8, SeekOrigin.Current);

                    //Check if we have exth record
                    var exthBitField = reader.ReadUInt32(Endian.BigEndian);
                    header._exthEnabled = ((exthBitField & 0x40) == 0x40);

                    if (length > 0x80)
                    {
                        byte[] data = new byte[32];

                        reader.Read(data, 0, 32);

                        // DRM Offset	
                        var drmOffset = reader.ReadUInt32(Endian.BigEndian);
                        //	DRM Count	
                        var drmCount = reader.ReadUInt32(Endian.BigEndian);
                        //	DRM Size	
                        var drmSize = reader.ReadUInt32(Endian.BigEndian);
                        //	DRM Flags
                        var drmFlags = reader.ReadUInt32(Endian.BigEndian);

                        header.Drm = new MobipocketDrm(drmOffset, drmCount, drmSize, drmFlags);
                        header.Drm.Parse(bytes);
                    }

                    //read trailing byte flags if present
                    if (length > 228)
                    {
                        reader.BaseStream.Seek(0xf2, SeekOrigin.Begin);

                        header.TrailingEntries = reader.ReadUInt16(Endian.BigEndian);
                    }

                    //read full name
                    reader.BaseStream.Seek(header._fullNameOffset, SeekOrigin.Begin);

                    var nameBuffer = new byte[header._fullNameLength];
                    checked { reader.Read(nameBuffer, 0, (int)header._fullNameLength); }
                    header.FullName = header.GetString(nameBuffer);


                    //mobi header end

                    if (header._exthEnabled)
                    {
                        //exth header start
                        //header.Data, length + 16
                        checked
                        {
                            Exth = ExthHeader.FromBinary(new ArraySegment<byte>(bytes.Array,
                                bytes.Offset + (int)length + 16,
                                bytes.Count - (int)length - 16));
                        }
                        //exth headet end
                    }
                }

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("cannot parse mobipocket header from binary", e);
            }
        }

        #endregion

        //---------------------------------------------
        //
        // Properties
        //
        //---------------------------------------------

        #region Properties

        /// <summary>
        /// Gets or sets unique id
        /// </summary>
        public uint UniqueId { get; set; }

        /// <summary>
        /// Gets or sets full ebook name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets minimal reader version needed to read document
        /// </summary>
        public uint MinReaderVersion { get; set; }

        /// <summary>
        /// Gets or sets local of ebook
        /// </summary>
        public uint Locale { get; set; }

        /// <summary>
        /// Gets or sets mobipocket dictionary info
        /// </summary>
        public MobipocketDictionaryInfo DictionaryInfo { get; set; }

        /// <summary>
        /// Gets or sets huffman information associated with mobipocket
        /// </summary>
        public MobipocketHuffmanInfo HuffmanInfo { get; set; }

        /// <summary>
        /// Gets or sets compression method
        /// </summary>
        public MobipocketCompression Compression { get; set; }

        /// <summary>
        /// Gets or sets encryption method of mobipocket
        /// </summary>
        public MobipocketEncryption Encryption { get; set; }

        /// <summary>
        /// Gets or sets type of mobipocket book
        /// </summary>
        public MobipocketType Type { get; set; }

        /// <summary>
        /// Gets or sets encoding of mobipocket
        /// </summary>
        public MobipocketEncoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets drm data
        /// </summary>
        public MobipocketDrm Drm { get; set; }

        /// <summary>
        /// Gets or sets verion of mobipocket document
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Gets or sets value indicating what trailing entries are present
        /// </summary>
        public ushort TrailingEntries { get; set; }

        /// <summary>
        /// Gets or sets first image record.
        /// </summary>
        public uint FirtImageRecord { get; set; }

        /// <summary>
        /// Gets or sets first no text record.
        /// </summary>
        public uint FirstNonTextRecord { get; set; }

        /// <summary>
        /// Gets or sets total text length of ebook.
        /// </summary>
        public uint TotalTextLength { get; set; }

        /// <summary>
        /// Gets or sets maximum record size of text record.
        /// </summary>
        public ushort MaxRecordSize { get; set; }

        /// <summary>
        /// Gets or sets exth header
        /// </summary>
        public static ExthHeader Exth { get; set; }

        /// <summary>
        /// Gets or sets total count of text records.
        /// </summary>
        public ushort TextRecordCount { get; set; }

        #endregion

        //---------------------------------------------
        //
        // Public methods
        //
        //---------------------------------------------

        #region Public methods

        public byte[] ToBinary()
        {
            throw new NotImplementedException();
        }

        #endregion

        //---------------------------------------------
        //
        // Private methods
        //
        //---------------------------------------------

        #region Private methods

        #endregion


        private string GetString(byte[] bytes)
        {
            return GetString(bytes, Encoding);
        }

        private static string GetString(byte[] bytes, MobipocketEncoding encoding)
        {
            switch (encoding)
            {
                case MobipocketEncoding.Latin1:
                    return System.Text.Encoding.GetEncoding("Latin1").GetString(bytes);
                case MobipocketEncoding.UTF8:
                    return System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes);
                default:
                    throw new NotSupportedException("encoding is not supported");
            }
        }




    }
}
