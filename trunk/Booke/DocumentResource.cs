using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booke
{
    /// <summary>
    /// Represent an resource.
    /// </summary>
    public class DocumentResource<T>
    {
        private T _type;
        private object _data;

        public DocumentResource(T type, object data)
        {
            this._type = type;
            this._data = data;
        }

        /// <summary>
        /// Gets data contained in resource.
        /// </summary>
        public object Data { get { return _data; } }

        /// <summary>
        /// Gets type of resource.
        /// </summary>
        public T Type { get { return _type; } }
    }
}
