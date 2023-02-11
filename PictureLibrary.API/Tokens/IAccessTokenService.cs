using PictureLibrary.Model;
using System.Security.Claims;

namespace PictureLibrary.API
{
    public interface IAccessTokenService
    {
        /// <summary>
        /// Generates tokens for specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Tokens GenerateTokens(User user);

        /// <summary>
        /// Retrieves ClaimsPrincipal from expired token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
