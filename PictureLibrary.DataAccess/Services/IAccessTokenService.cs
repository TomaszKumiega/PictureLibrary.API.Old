using Microsoft.IdentityModel.Tokens;
using PictureLibrary.Model;

namespace PictureLibrary.DataAccess.Services
{
    public interface IAccessTokenService
    {
        /// <summary>
        /// Generates tokens for specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Tokens GenerateTokens(User user, string privateKey);

        /// <summary>
        /// Generates new tokens.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="accessToken">Expired access token</param>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns></returns>
        Task<Tokens> RefreshTokensAsync(TokenValidationParameters tokenValidationParams, string accessToken, string refreshToken, string privateKey);
    }
}
