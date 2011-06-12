using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket.PalmDatabase
{
    [Flags]
    internal enum PalmRecordAttributes
    {
        None = 0,
        SecretRecordBit = 1,
        RecordInUse = 2,
        DirtyRecord = 4,
        DeleteOnNextHotSynx = 8
    }
}
