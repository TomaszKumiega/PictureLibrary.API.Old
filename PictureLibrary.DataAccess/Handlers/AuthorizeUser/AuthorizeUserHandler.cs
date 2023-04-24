using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;
using PictureLibrary.Tools;

namespace PictureLibrary.DataAccess.Handlers.AuthorizeUser
{
    public class AuthorizeUserHandler : IRequestHandler<AuthorizeUserQuery, Tokens?>
    {
        private readonly IHashAndSalt _hashAndSaltHelper;
        private readonly IUserRepository _userRepository;
        private readonly IAccessTokenService _accessTokenService;

        public AuthorizeUserHandler(
            IHashAndSalt hashAndSaltHelper,
            IUserRepository userRepository,
            IAccessTokenService accessTokenService)
        {
            _userRepository = userRepository;
            _hashAndSaltHelper = hashAndSaltHelper;
            _accessTokenService = accessTokenService;
        }

        public async Task<Tokens?> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByUsername(request.Username);

            if (user == null || _hashAndSaltHelper.VerifyHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return _accessTokenService.GenerateTokens(user, request.PrivateKey);
        }
    }
}
