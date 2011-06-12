using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket.PalmDatabase
{
    /// <summary>
    /// Dalm database attributes as described in
    /// http://wiki.mobileread.com/wiki/PDB#Palm_Database_Format.
    /// </summary>
    [Flags]
    internal enum PalmDatabaseAttributes
    {
        None=0,
        ReadOnly = 0x02,
        DirtyAppInfoArea = 0x04,
        Backup = 0x08,
        OkayToInstallNewerCopy = 0x10,
        ForceResetAfterInstall = 0x20,
        DontAllowCopy = 0x40
    }
}
