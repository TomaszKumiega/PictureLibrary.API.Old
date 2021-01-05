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
            var token = _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == refreshToken)
                .FirstOrDefault();
            _context.RefreshTokens.Remove(token);
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
            var token = _context.RefreshTokens
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
            
            return token.Token;
        }

        public void SaveRefreshToken(string userId, string refreshToken)
        {
            var refToken = new RefreshToken();

            refToken.Id = Guid.NewGuid();
            refToken.UserId = userId;
            refToken.Token = refreshToken;

            _context.RefreshTokens.Add(refToken);
            _context.SaveChanges();
        }
    }
}
