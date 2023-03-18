using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record DeleteLibraryCommand(Guid LibraryId) : IRequest;
}
