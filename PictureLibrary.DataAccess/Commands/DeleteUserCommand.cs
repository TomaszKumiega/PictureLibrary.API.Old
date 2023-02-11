using MediatR;

namespace PictureLibrary.DataAccess.Commands
{
    public record DeleteUserCommand(string Username) : IRequest;
}
