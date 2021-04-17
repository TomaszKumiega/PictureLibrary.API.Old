using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public interface IAccessTokenService
    {
        string GenerateToken();
        string GetRefreshToken(string userId);
        void SaveRefreshToken(string userId, string refreshToken);
        void DeleteRefreshToken(string userId, string refreshToken);
        string GenerateToken(string userId);
    }
}
