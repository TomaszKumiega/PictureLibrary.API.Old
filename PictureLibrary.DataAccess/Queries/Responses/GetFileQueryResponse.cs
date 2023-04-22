using PictureLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureLibrary.DataAccess.Queries.Responses
{
    public record GetFileQueryResponse(ImageFile ImageFile, FileStream FileStream);
}
