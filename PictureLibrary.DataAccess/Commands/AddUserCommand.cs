using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record AddUserCommand(string? Username, string? EmailAddress, string? Password) : IRequest<Guid>;
}
