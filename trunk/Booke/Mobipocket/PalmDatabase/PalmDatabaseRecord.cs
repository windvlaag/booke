using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke.Mobipocket.PalmDatabase
{
    internal class PalmDatabaseRecord
    {
        /// <summary>
        /// Gets or sets data in record.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets palm record attributes.
        /// </summary>
        public PalmRecordAttributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets unique id of record.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets category of record (values 1-16)
        /// </summary>
        public int Category { get; set; }

        public uint Offset { get; set; }

        public uint Size { get; set; }
    }
}
