using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke.Mobipocket
{
    /// <summary>
    /// Provides know data types for eth records.
    /// </summary>
    public static class ExthRecordTypes
    {
        //---------------------------------------------
        //
        // Known record types
        //
        //---------------------------------------------

        #region Known record types

        // Those values are constant
        // instead of Enum type to
        // avoid errors on unknown types

        public const uint
            DrmServerId	= 1,
            DrmCommerceId = 2,
            DrmEbookbaseBookId = 3,
            Author = 100,
            Publisher = 101,
            Imprint = 102,
            Description = 103,
            Isbn = 104,
            Subject = 105,
            PublishingDate = 106,	
            Review = 107,
            Contributor = 108,
            Rights = 109,
            SubjectCode = 110,
            Type = 111,
            Source = 112,
            Asin = 113,
            VersionNumber = 114,	
            Sample = 115,
            StartReading = 116,
            Adult = 117,
            RetailPrice = 118,
            RetailPriceCurrency	= 119,
            CoverOffset = 201,
            ThumbOffset = 202,
            HasFakeCover = 203,
            CreatorSoftware = 204,
            CreatorMajorVersion = 205,	
            CreatorMinorVersion = 206,
            CreatorBuildNumber = 207,
            Watermark = 208,
            TamperProofKeys = 209,
            FontSignature = 300,
            ClippingLimit = 401,
            PublisherLimit = 402,
            TextToSpeachFlag = 404,
            CdeType = 501,
            LastUpdateTime = 502,	
            UpdatedTitle = 503;

        #endregion
    }
}
