using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record GetUserLibrariesQuery(Guid UserId) : IRequest<IEnumerable<Library>>;
}
