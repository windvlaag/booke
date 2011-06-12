using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Booke
{
    /// <summary>
    /// Document that would be used by writer.
    /// </summary>
    /// <typeparam name="T">Enum type of resources in document</typeparam>
    public abstract class Document<T>
    {
        private List<DocumentResource<T>> _resources = new List<DocumentResource<T>>(); 

        protected void AddResource(T type, object data)
        {
            if(data == null)
                throw new ArgumentNullException("data");

            _resources.Add(
                new DocumentResource<T>(type, data)
            );
        }

        protected void AddResources(IEnumerable<DocumentResource<T>> resources)
        {
            if (resources == null)
                throw new ArgumentNullException("resources");

            _resources.AddRange(resources);
        }

        protected IEnumerable<DocumentResource<T>> Resources { get { return _resources; } }

        /// <summary>
        /// This method implemented in every descendant, should return whole ready to save document.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] Construct();

        /// <summary>
        /// Saves document to file.
        /// </summary>
        /// <param name="file">Open file stream with writing access.</param>
        public void SaveToFile(FileStream file)
        {
            if(file == null)
                throw new ArgumentNullException("file");

            byte[] bytes;

            //contruct the file.
            try
            {
                bytes = Construct();

                if( bytes == null )
                    throw new Exception("Contruct() method returned null");

            }
            catch(Exception e)
            {
                throw new DocumentConstructionException("Exception caught when contructiong document.", e);
            }

            try
            {
                file.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                throw new DocumentConstructionException("Exception caught when writing document to file.", e);
            }
        }

        /// <summary>
        /// Saves document to file.
        /// </summary>
        /// <param name="path">Path to a file.</param>
        public void SaveToFile(string path)
        {
            if (string.IsNullOrWhiteSpace("path"))
                throw new ArgumentException("path argument is incorrect.");

            try
            {
                using (var file = File.OpenWrite(path))
                {
                    SaveToFile(file);
                }
            }
            catch (Exception e)
            {
                throw new DocumentConstructionException("Cannot access file '" + path + "'", e);
            }

        }
    }
}
