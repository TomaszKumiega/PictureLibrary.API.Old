using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record GetImageFilesQuery(Guid LibraryId, Guid UserId) : IRequest<IEnumerable<ImageFile>>;
}
