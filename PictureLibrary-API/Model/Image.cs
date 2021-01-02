using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Model
{
    public class Image
    {
        public ImageFile ImageFile { get; set; }
        public byte[] ImageContent { get; set; }
    }
}
