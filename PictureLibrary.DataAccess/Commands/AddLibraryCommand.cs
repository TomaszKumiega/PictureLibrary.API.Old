using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record AddLibraryCommand(Library Library, Guid UserId) : IRequest<Guid>;
}
