using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record UpdateTagCommand(Guid UserId, Guid TagId, string? Name, string? Description, string? ColorHex, IEnumerable<Guid>? Libraries) : IRequest;
}
