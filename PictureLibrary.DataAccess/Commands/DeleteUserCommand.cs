using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record DeleteUserCommand(Guid userId, string Username) : IRequest;
}
