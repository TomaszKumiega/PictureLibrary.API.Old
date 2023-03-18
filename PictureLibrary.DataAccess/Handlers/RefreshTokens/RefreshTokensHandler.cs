using MediatR;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.DataAccess.Services;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Handlers.RefreshTokens
{
    public class RefreshTokensHandler : IRequestHandler<RefreshTokensQuery, Tokens>
    {
        private readonly IAccessTokenService _accessTokenService;

        public RefreshTokensHandler(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        public async Task<Tokens> Handle(RefreshTokensQuery request, CancellationToken cancellationToken)
            => await _accessTokenService.RefreshTokensAsync(request.TokenValidationParams, request.AccessToken!, request.RefreshToken!, request.PrivateKey!);
    }
}
