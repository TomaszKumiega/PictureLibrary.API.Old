using Microsoft.IdentityModel.Tokens;
using PictureLibrary_API.Helpers;
using PictureLibrary_API.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PictureLibrary_API.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private DatabaseContext Context { get; }
        private AppSettings AppSettings { get; }

        public AccessTokenService(DatabaseContext context, AppSettings appSettings)
        {
            Context = context;
            AppSettings = appSettings;
        }

        public void DeleteRefreshToken(string userId, string refreshToken)
        {
            var token = Context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == refreshToken)
                .FirstOrDefault();
            Context.RefreshTokens.Remove(token);
            Context.SaveChanges();
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
            var token = Context.RefreshTokens
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

            Context.RefreshTokens.Add(refToken);
            Context.SaveChanges();
        }

        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
