using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Commands
{
    public record SaveTokensCommand(Tokens Tokens) : IRequest;
}
