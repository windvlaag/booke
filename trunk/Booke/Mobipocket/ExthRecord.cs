using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Single record of exth header
    /// </summary>
    public class ExthRecord
    {
        /// <summary>
        /// Creates new exth record.
        /// </summary>
        public ExthRecord()
            : this(0)
        {
        }
            
        /// <summary>
        /// Creates new exth record with privded type.
        /// </summary>
        /// <param name="type"></param>
        public ExthRecord(uint type)
            : this(type, null)
        {
        }

        /// <summary>
        /// Creates new exth record with provided type and data.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public ExthRecord(uint type, byte[] data)
        {
            this.Type = type;
            this.Data = data;
        }


        /// <summary>
        /// Gets or sets record type. 
        /// </summary>
        /// <remarks>
        /// Documented at
        /// http://wiki.mobileread.com/wiki/MOBI#EXTH%20Header
        /// </remarks>
        public uint Type { get; set; }

        /// <summary>
        /// Gets or sets record binary data
        /// </summary>
        public byte[] Data { get; set; }
    }
}
