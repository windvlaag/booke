using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    public class TrailingEntryCollection : IEnumerable<TrailingEntry>
    {

        public TrailingEntryCollection()
        {

        }

        public byte[] ToBinary()
        {
            throw new NotImplementedException();
        }


        private List<TrailingEntry> _entries = new List<TrailingEntry>();

        /// <summary>
        /// Gets or sets byte count of all entries.
        /// </summary>
        public int ByteCount { get; set; }

        /// <summary>
        /// Add new trailing entry
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="bytes"></param>
        public void AddEntry(ushort flag, byte[] bytes)
        {
            AddEntry(new TrailingEntry { Flag = flag, Data = bytes });
        }

        /// <summary>
        /// Add new trailing entry
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(TrailingEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            _entries.Add(entry);
        }

        //---------------------------------------------
        //
        // IEnumerble implementation
        //
        //---------------------------------------------

        #region IEnumerble implementation

        public IEnumerator<TrailingEntry> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        #endregion


        /// <summary>
        /// Whether there is multibyte character overlap in this record.
        /// </summary>
        public bool IsMultibyteCharacterOverlapping { get { return MultibyteCharacterOverlap != null && MultibyteCharacterOverlap.Length > 0; } }

        public byte[] MultibyteCharacterOverlap { get; set; }
    }
}
