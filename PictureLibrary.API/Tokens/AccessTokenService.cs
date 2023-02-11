using Microsoft.IdentityModel.Tokens;
using PictureLibrary.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PictureLibrary.API
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IConfiguration _config;

        public AccessTokenService(IConfiguration config)
        {
            _config = config;
        }

        #region GenerateTokens
        public Tokens GenerateTokens(User user)
        {
            var (accessToken, expiryDate) = GenerateAccessToken(user);
            return new()
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken(),
                ExpiryDate = expiryDate,
            };
        }

        private (string accessToken, DateTime expiryDate) GenerateAccessToken(User user)
        {
            DateTime expires = DateTime.UtcNow.AddHours(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GetPrivateKey());
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
        #endregion

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new PictureLibraryTokenValidationParameters(_config);
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is JwtSecurityToken jwtSecurityToken 
                && !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private string GetPrivateKey()
            => _config["PrivateKey"]!;
    }
}
