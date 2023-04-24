using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record UpdateLibraryCommand(Library Library) : IRequest;
}
