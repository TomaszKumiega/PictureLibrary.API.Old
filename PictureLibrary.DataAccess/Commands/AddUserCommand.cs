using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record AddUserCommand(UserRegister User) : IRequest;
}
