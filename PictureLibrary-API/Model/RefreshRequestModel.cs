using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Model
{
    public class RefreshRequestModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
