using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record CreateUploadSessionCommand(Guid UserId, string FileName, string ContentRange, List<Guid> LibraryIds) : IRequest<Guid>;
}
