using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record UploadFileCommand(Guid UserId, Guid UploadSessionId, byte[] Buffer, string ContentRange, int BytesRead) : IRequest<ImageFile?>;
}
