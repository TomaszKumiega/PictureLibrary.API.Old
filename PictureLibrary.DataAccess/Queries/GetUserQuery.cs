using MediatR;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record GetUserQuery(Guid UserId) : IRequest<User>;
}
