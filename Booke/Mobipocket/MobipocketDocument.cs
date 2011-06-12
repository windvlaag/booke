using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Booke.Mobipocket.PalmDatabase;
using System.Drawing;
using System.Drawing.Imaging;

namespace Booke.Mobipocket
{
    public sealed class MobipocketDocument : Document<MobipocketResourceTypes>, ITextWriter
    {
        //---------------------------------------------
        //
        // Private fields
        //
        //---------------------------------------------

        #region Private fields

        // "MOBI" in hex
        private const uint MobiIdentifier = 0x4d4f4249;
        private PalmDatabaseFormat _pdb;
        private readonly LZ77Decoder _lz77decoder = new LZ77Decoder();
        private List<MobipocketRecord> _records = new List<MobipocketRecord>();

        #endregion

        //---------------------------------------------
        //
        // .ctor
        //
        //---------------------------------------------

        #region .ctor

        public MobipocketDocument()
            : this(MobipocketEncoding.Latin1, MobipocketCompression.LZ77)
        { }

        public MobipocketDocument(MobipocketEncoding encoding)
            : this(encoding, MobipocketCompression.LZ77)
        { }

        public MobipocketDocument(MobipocketEncoding encoding, MobipocketCompression compression)
        {
            //create new pdb
            this._pdb = new PalmDatabaseFormat();

            //create new header
            this.Header = new MobipocketHeader();

            //set default properties
            this.Compression = compression;
            this.Encoding = encoding;
        }

        private MobipocketDocument(PalmDatabaseFormat pdb)
        {
            if (pdb == null)
                throw new ArgumentNullException("pdb");

            if (pdb.Records.Count < 1)
                throw new Exception("palm database must contain at least one record");

            //set pdb
            this._pdb = pdb;

            //parse header
            this.Header = MobipocketHeader.FromPalmDatabase(_pdb);

            //parse records
            _records.AddRange(ParseMobipocketRecords());

        }

        #endregion

        //---------------------------------------------
        //
        // Static constructors
        //
        //---------------------------------------------

        #region Static constructors

        /// <summary>
        /// Reads file into mobipocket document object
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MobipocketDocument FromFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            try
            {

                using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(file))
                {

                    // read pdb format file
                    var db = PalmDatabaseFormat.FromBinary(reader);

                    //create the document from pdb
                    return new MobipocketDocument(db);
                }
            }
            catch (Exception e)
            {
                throw new DocumentConstructionException("Cannot load mobipocket document from file", e);
            }


        }

        #endregion

        //---------------------------------------------
        //
        // Public properties
        //
        //---------------------------------------------

        #region Public properties

        /// <summary>
        /// Gets or sets full name of mobipocket document.
        /// </summary>
        public string Name
        {
            get { return Header.FullName; }
            set { Header.FullName = value; }
        }

        /// <summary>
        /// Gets or sets mobipocket encryption
        /// </summary>
        public MobipocketEncryption Encryption
        {
            get { return Header.Encryption; }
            set
            {
                if (value != MobipocketEncryption.NoEncryption)
                    throw new NotSupportedException("encryption not supported");

                Header.Encryption = value;
            }
        }

        /// <summary>
        /// Gets or sets mobipocket type
        /// </summary>
        public MobipocketType Type
        {
            get { return Header.Type; }
            set { Header.Type = value; }
        }

        /// <summary>
        /// Gets and sets date for ebook creation.
        /// </summary>
        public DateTime Created
        {
            get { return _pdb.CreationDate; }
            set { _pdb.CreationDate = value; }
        }

        /// <summary>
        /// Gets and sets modification date for ebook.
        /// </summary>
        public DateTime Modified
        {
            get { return _pdb.ModificationDate; }
            set { _pdb.ModificationDate = value; }
        }


        /// <summary>
        /// Gets and sets used compression methods.
        /// </summary>
        public MobipocketCompression Compression
        {
            get { return Header.Compression; }
            set
            {
                if (value == MobipocketCompression.Huffdic)
                    throw new NotSupportedException("huffdic compression not supported");

                Header.Compression = value;
            }
        }

        /// <summary>s
        /// Gets and sets encoding used in mobipocket.
        /// </summary>
        public MobipocketEncoding Encoding
        {
            get { return Header.Encoding; }
            set { Header.Encoding = value; }
        }


        /// <summary>
        /// Gets or sets mobipocket file version.
        /// </summary>
        public uint Version
        {
            get { return Header.Version; }
            set { Header.Version = value; }
        }

        /// <summary>
        /// Gets header object for mobipocket documnet
        /// </summary>
        public MobipocketHeader Header { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<MobipocketRecord> Records
        {
            get { return _records; }
        }

        #endregion

        //---------------------------------------------
        //
        // Public methods
        //
        //---------------------------------------------

        #region Public methods

        /// <summary>
        /// Gets System.Encoding object used by mobipocket for saving text data.
        /// </summary>
        /// <returns></returns>
        public Encoding GetSystemEncoding()
        {
            switch (Encoding)
            {
                case MobipocketEncoding.Latin1:
                    return System.Text.Encoding.GetEncoding("Latin1");
                case MobipocketEncoding.UTF8:
                    return System.Text.Encoding.GetEncoding("UTF-8");
                default:
                    throw new NotSupportedException("encoding is not supported");
            }
        }

        public void AdjustHeaderData()
        {
        }

        #endregion

        //---------------------------------------------
        //
        // Document abstract implementation
        //
        //---------------------------------------------

        #region Document abstract implementation

        public override byte[] Construct()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                //



                //write header
                throw new NotImplementedException();

                //return stream.ToArray();
            }
        }

        #endregion

        //---------------------------------------------
        //
        // ITextWriter
        //
        //---------------------------------------------

        #region ITextWriter

        public void WriteText(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            AddResource(MobipocketResourceTypes.Text, GetSystemEncoding().GetBytes(text));
        }

        public void WriteText(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            WriteText(string.Format(format, args));
        }

        public IEnumerable<string> Text
        {
            get
            {
                return from resource in Resources
                       where resource.Type == MobipocketResourceTypes.Text
                       select resource.Data as string;
            }
        }

        public string GetFullText()
        {
            return string.Concat(Text);
        }

        #endregion

        //---------------------------------------------
        //
        // Private methods
        //
        //---------------------------------------------

        #region Private methods

        private IEnumerable<MobipocketRecord> ParseMobipocketRecords()
        {
            foreach (var record in ParseTextMobipocketRecords())
            {
                AddResource(MobipocketResourceTypes.Text, record.Text);
                yield return record;
            }

            for (int index = (int)Header.FirstNonTextRecord; index < _pdb.Records.Count; index++)
            {
                var data = _pdb.Records[index].Data;
                var record = MobipocketRecord.FromBinary(data);

                yield return record;

                if (record.IsFLISRecord)
                    AddResource(MobipocketResourceTypes.FLIS, data);
                else if (record.IsFCISRecord)
                    AddResource(MobipocketResourceTypes.FCIS, data);
                else if (record.IsEOFRecord)
                    AddResource(MobipocketResourceTypes.EOF, data);
                else

                    try
                    {

                        using (var stream = new MemoryStream(record.Data))
                        using (var image = Image.FromStream(stream))
                        {
                            var format = image.RawFormat;

                            if (format.Equals(ImageFormat.Jpeg) ||
                                format.Equals(ImageFormat.Gif) ||
                                format.Equals(ImageFormat.Bmp))

                                AddResource(MobipocketResourceTypes.Image, data);

                            else
                                throw new NotSupportedException("image not supported");

                        }
                    }
                    catch
                    {
                         AddResource(MobipocketResourceTypes.Binary, data);
                    }
            }
        }

        /// <summary>
        /// Parses all text record
        /// </summary>
        private IEnumerable<MobipocketRecord> ParseTextMobipocketRecords()
        {
            int bytesUsed = 0;

            var parser = new TrailingEntryParser(Header.TrailingEntries);
            int previousOverlapSize = 0;

            //skip first record as header, and process all text records
            for (int recordIndex = 0; recordIndex < Header.TextRecordCount; recordIndex++)
            {
                //get current record
                var record = _pdb.Records[recordIndex + 1];

                //read record
                var mobiRecord = MobipocketRecord.FromBinary(record.Data, true, parser);

                //uncompress text
                mobiRecord.Text = GetRecordText(mobiRecord, previousOverlapSize, ref bytesUsed);

                //yield record
                yield return mobiRecord;

                //save trailing entries
                previousOverlapSize = mobiRecord.TrailingEntries.IsMultibyteCharacterOverlapping ?
                    mobiRecord.TrailingEntries.MultibyteCharacterOverlap.Length : 0;
            }

            if (bytesUsed != Header.TotalTextLength)
                throw new Exception("text length doesn't match used characters");
        }


        /// <summary>
        /// Parses all image records
        /// </summary>
        private IEnumerable<DocumentResource<MobipocketResourceTypes>> ParseImageMobipocketRecords()
        {
            for (uint i = Header.FirtImageRecord; i < _pdb.Records.Count; i++)
            {
                checked { yield return new DocumentResource<MobipocketResourceTypes>(MobipocketResourceTypes.Image, _pdb.Records[(int)i].Data); }
            }
        }

        /// <summary>
        /// Gets string value associated with palm database record using given compression schemes.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private string GetRecordText(MobipocketRecord record, int previousOverlapSize, ref int bytesUsed)
        {
            if (record == null)
                throw new ArgumentException("record");

            //resulting data
            byte[] resultData;

            //decompress the text
            switch (Compression)
            {
                case MobipocketCompression.NoCompression:
                    {
                        var segment = record.GetDataWithoutTrailingEntries();
                        resultData = new byte[segment.Count];

                        Array.Copy(segment.Array, segment.Offset, resultData, 0, segment.Count);
                        break;
                    }

                case MobipocketCompression.LZ77:
                    resultData = GetLZ77DecodedText(record.GetDataWithoutTrailingEntries());
                    break;
                case MobipocketCompression.Huffdic:
                    throw new NotSupportedException("huffdic compression not supported yet");
                default:
                    throw new NotSupportedException();
            }

            //adjust bytes used
            bytesUsed += resultData.Length;

            //add Multibyte character overlap entry to text
            if (record.TrailingEntries.IsMultibyteCharacterOverlapping)
            {
                //may be not the most proficient method
                resultData = resultData.Concat(record.TrailingEntries.MultibyteCharacterOverlap).ToArray();
            }

            return GetSystemEncoding().GetString(resultData, previousOverlapSize, resultData.Length - previousOverlapSize);
        }

        /// <summary>
        /// Gets text decoded using lz77.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private byte[] GetLZ77DecodedText(ArraySegment<byte> bytes)
        {
            if (bytes.Array == null)
                throw new ArgumentException("bytes array cannot be null");

            //get count of bytes after decoding
            int size = _lz77decoder.GetByteCount(bytes.Array, bytes.Offset, bytes.Count);

            //check if size doesn't exceed max length
            if (size > Header.MaxRecordSize)
                throw new Exception("Text record cannot contain more than " + Header.MaxRecordSize + " characters");

            //create buffer for bytes
            byte[] decodedBytes = new byte[size];

            //decode the text
            var realSize = _lz77decoder.GetBytes(bytes.Array, bytes.Offset, bytes.Count, decodedBytes, 0);

            //return text after conversion
            return decodedBytes;
        }

        #endregion




    }
}
