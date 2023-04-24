using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries.Responses
{
    public record GetFileQueryResponse(ImageFile ImageFile, FileStream FileStream);
}
