using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    public enum MobipocketCompression
    {
        /// <summary>
        /// No compression methds were used.
        /// </summary>
        NoCompression = 1,

        /// <summary>
        /// LZ77 standard compression methods were used.
        /// </summary>
        LZ77 = 2,

        /// <summary>
        /// Proprietary Huffdic compression methods were used.
        /// </summary>
        Huffdic = 17480
    }
}
