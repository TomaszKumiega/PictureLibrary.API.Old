using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record FindUserQuery(string Username) : IRequest<User?>;
}
