using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private DatabaseContext _context;

        public RefreshTokenService(DatabaseContext context)
        {
            _context = context;
        }

        public void DeleteRefreshToken(string userId, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public string GetRefreshToken(string userId)
        {
            throw new NotImplementedException();
        }

        public void SaveRefreshToken(string userId, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
