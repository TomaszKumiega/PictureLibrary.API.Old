using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers
{
    public class FindUserHandler : IRequestHandler<FindUserQuery, User>
    {
        public Task<User> Handle(FindUserQuery request, CancellationToken cancellationToken)
        {
            // find username

            return null!;
        }
    }
}
