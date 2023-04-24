using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record FindLibraryByIdQuery(Guid LibraryId) : IRequest<Library?>;
}
