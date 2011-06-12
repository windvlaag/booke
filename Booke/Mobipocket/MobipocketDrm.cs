using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Drm class for mobipocket protection
    /// </summary>
    /// <remarks>
    /// This feature is not implemented
    /// </remarks>
    public class MobipocketDrm
    {
        public MobipocketDrm()
        {

        }

        public MobipocketDrm(uint drmOffset, uint drmCount, uint drmSize, uint drmFlags)
        {

            this.Offset = drmOffset;
            this.Count = drmCount;
            this.Size = drmSize;
            this.Flags = drmFlags;
        }

        /// <summary>
        /// Gets or sets offset of drm data
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Gets or sets byte count of drm data
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Gets or sets size of drm data
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// Gets or sets flags for drm protection
        /// </summary>
        public uint Flags { get; set; }

        /// <summary>
        /// Reads data from binary using provided offset,count and size values
        /// </summary>
        /// <param name="bytes"></param>
        public void Parse(ArraySegment<byte> bytes)
        {
            if (Offset != 0xffffffff || Count != 0xffffffff)
                throw new NotSupportedException("drm is not supported");
        }
    }
}
