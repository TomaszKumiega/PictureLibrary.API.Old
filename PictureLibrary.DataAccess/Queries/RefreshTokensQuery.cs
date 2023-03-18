using MediatR;
using Microsoft.IdentityModel.Tokens;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Queries
{
    public record RefreshTokensQuery(TokenValidationParameters TokenValidationParams, string? AccessToken, string? RefreshToken, string? PrivateKey) : IRequest<Tokens>;
}
