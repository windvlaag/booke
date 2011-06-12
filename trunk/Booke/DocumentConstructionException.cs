using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke
{
    [Serializable]
    public class DocumentConstructionException : Exception
    {
        public DocumentConstructionException() { }
        public DocumentConstructionException(string message) : base(message) { }
        public DocumentConstructionException(string message, Exception inner) : base(message, inner) { }
        protected DocumentConstructionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
