using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.FindUsers
{
    public class FindUsersHandler : IRequestHandler<FindUsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _userRepository;

        public FindUsersHandler(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;    
        }

        public async Task<IEnumerable<User>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.FindByPartialUsername(request.Username);
        }
    }
}
