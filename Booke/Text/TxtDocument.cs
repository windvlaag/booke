using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke.Text
{
    public class TxtDocument : Document<TxtResourceTypes>, ITextWriter
    {
        private Encoding _encoding;

        /// <summary>
        /// Creates new txt document with default system encoding.
        /// </summary>
        public TxtDocument()
            :this (Encoding.Default)
        {

             
        }

        /// <summary>
        /// Creates new txt document with custom encoding.
        /// </summary>
        /// <param name="encoding"></param>
        public TxtDocument(Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            this._encoding = encoding;
        }

        /// <summary>
        /// Constructs txt file.
        /// </summary>
        /// <returns></returns>
        public override byte[] Construct()
        {
            using(var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var q = from res in Resources
                        where res.Type == TxtResourceTypes.Text
                        select res.Data as string;

                foreach (var text in q)
                    writer.Write(_encoding.GetBytes(text));

                return stream.ToArray();
            }
        }

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

            AddResource(TxtResourceTypes.Text, text);
        }

        public void WriteText(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            if (args == null)
                throw new ArgumentNullException("args");

            WriteText(string.Format(format, args));
        }


        public IEnumerable<string> Text
        {
            get
            {
                return from res in Resources
                       where res.Type == TxtResourceTypes.Text
                       select res.Data as string;
            }
        }

        public string GetFullText()
        {
            return string.Concat(Text);
        }

        #endregion


    }
}
