using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke
{
    public interface ITextWriter
    {
        /// <summary>
        /// Writes text.
        /// </summary>
        /// <param name="text"></param>
        void WriteText(string text);

        /// <summary>
        /// Writes text parsed using string.Format.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void WriteText(string format, params object[] args);

        /// <summary>
        /// Gets all written text in chunks.
        /// </summary>
        IEnumerable<string> Text { get; }

        /// <summary>
        /// Gets all text.
        /// </summary>
        /// <returns></returns>
        string GetFullText();
    }
}
