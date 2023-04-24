using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record DeleteImageFileCommand(Guid UserId, Guid ImageFileId) : IRequest;
}
