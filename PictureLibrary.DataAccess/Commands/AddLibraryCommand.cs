using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record AddLibraryCommand(NewLibrary Library, Guid UserId) : IRequest<Guid>;
}
