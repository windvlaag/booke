using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Dictionary information associated with mobipocket file
    /// </summary>
    public class MobipocketDictionaryInfo
    {
        /// <summary>
        /// Input language of dictionary
        /// </summary>
        public uint InputLanguage { get; set; }

        /// <summary>
        /// Output language of dictionary
        /// </summary>
        public uint OutputLanguage { get; set; }
    }
}
