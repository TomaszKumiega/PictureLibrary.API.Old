using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record UpdateImageFileCommand(Guid UserId, Guid ImageFileId, string Name) : IRequest;
}
