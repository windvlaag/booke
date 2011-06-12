using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Mobipocket huffman information.
    /// </summary>
    public class MobipocketHuffmanInfo
    {
        /// <summary>
        /// Gets or sets offset of huffman record
        /// </summary>
        public uint RecordOffset { get; set; }

        /// <summary>
        /// Gets or sets huffman record count
        /// </summary>
        public uint RecordCount { get; set; }
    }
}
