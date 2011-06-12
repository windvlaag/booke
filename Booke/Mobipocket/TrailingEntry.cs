using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    public class TrailingEntry
    {
        public const ushort MultibyteCharacterOverlapFlag = 0x0001;


        public ushort Flag { get; set; }

        public byte[] Data { get; set; }
    }
}
