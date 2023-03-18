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
    }
}
