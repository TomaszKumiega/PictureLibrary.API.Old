using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Exceptions
{
    public class ContentNotFoundException : Exception
    {
        public ContentNotFoundException() : base()
        {

        }

        public ContentNotFoundException(string message) : base(message)
        {

        }

        public ContentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
