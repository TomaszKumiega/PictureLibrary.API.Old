using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record FindUsersQuery(string Username) : IRequest<IEnumerable<User>>;
}
