using Microsoft.IdentityModel.Tokens;
using PictureLibrary.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PictureLibrary.DataAccess.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        public Tokens GenerateTokens(User user, string privateKey)
        {
            var (accessToken, expiryDate) = GenerateAccessToken(user, privateKey);
            return new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken(),
                ExpiryDate = expiryDate,
            };
        }

        private static (string accessToken, DateTime expiryDate) GenerateAccessToken(User user, string privateKey)
        {
            DateTime expires = DateTime.UtcNow.AddHours(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(privateKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                }),

                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), expires);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
