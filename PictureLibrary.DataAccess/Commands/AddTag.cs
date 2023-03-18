using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record AddTagCommand(Guid UserId, string? Name, string? Description, string? ColorHex, List<Guid>? Libraries) : IRequest<Guid>;
}
