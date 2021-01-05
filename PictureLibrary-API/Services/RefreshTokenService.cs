using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            var token = _context.RefreshTokens.Where(x => x.UserId == userId && x.Token == refreshToken);
            _context.RefreshTokens.RemoveRange(token);
            _context.SaveChanges();
        }

        public string GenerateToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
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
