using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record UpdateUserCommand(User User) : IRequest;
}
