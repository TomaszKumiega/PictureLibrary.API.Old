using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers
{
    public class FindUserHandler : IRequestHandler<FindUserQuery, User?>
    {
        private readonly IUserRepository _userRepository;

        public FindUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;    
        }

        public async Task<User?> Handle(FindUserQuery request, CancellationToken cancellationToken)
            => await _userRepository.FindByUsername(request.Username);
    }
}
