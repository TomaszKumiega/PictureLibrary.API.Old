using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record DeleteUserCommand(Guid UserId) : IRequest;
}
