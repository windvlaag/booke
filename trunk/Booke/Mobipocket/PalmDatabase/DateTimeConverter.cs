using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket.PalmDatabase
{
    /// <summary>
    /// Provides methods for time stamp conversions.
    /// </summary>
    internal static class DateTimeConverter
    {
        /// <summary>
        /// Gets date time object from unix timestamp.
        /// </summary>
        /// <param name="Seconds">Timestamp in seconds.</param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(long Seconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Seconds).ToLocalTime();
        }

        /// <summary>
        /// Gets unix timestamp.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int ToUnixTimestamp(this DateTime dateTime)
        {
            if(dateTime == null)
                throw new ArgumentException("dateTime");

            throw new NotImplementedException();
        }


        public static DateTime GetPDBTimeFromTimestamp(uint timestamp)
        {
            // Look up documentation for prc for more info on this
            if ((0x80000000 & timestamp) == 0x80000000)
                return DateTimeConverter.FromMacTimestamp(timestamp);
            else
                return DateTimeConverter.FromUnixTimestamp(timestamp);
        }


        /// <summary>
        /// Gets date time object from mac os timestamp.
        /// </summary>
        /// <param name="Seconds">Timestamp in seconds.</param>
        /// <returns></returns>
        public static DateTime FromMacTimestamp(long Seconds)
        {
            return new DateTime(1904, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Seconds).ToLocalTime();
        }
    }
}
