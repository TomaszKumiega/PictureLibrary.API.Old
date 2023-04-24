using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record AuthorizeUserQuery(string Username, string Password, string PrivateKey) : IRequest<Tokens?>;
}
