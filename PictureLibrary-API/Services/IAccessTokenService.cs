using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public interface IAccessTokenService
    {
        /// <summary>
        /// Generates new refresh token.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
        /// <summary>
        /// Retrieves refresh token from database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetRefreshToken(string userId);
        /// <summary>
        /// Saves refresh token in database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        Task SaveRefreshToken(string userId, string refreshToken);
        /// <summary>
        /// Removes refresh token from database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        Task DeleteRefreshToken(string userId, string refreshToken);
        /// <summary>
        /// Generates new access token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GenerateAccessToken(string userId);
        /// <summary>
        /// Retrieves principal from expired token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
