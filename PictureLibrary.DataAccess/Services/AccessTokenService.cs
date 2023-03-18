using Microsoft.IdentityModel.Tokens;
using PictureLibrary.DataAccess.Exceptions;
using PictureLibrary.DataAccess.Repositories;
using PictureLibrary.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PictureLibrary.DataAccess.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokensRepository _tokensRepository;

        public AccessTokenService(
            IUserRepository userRepository,
            ITokensRepository tokensRepository)
        {
            _userRepository = userRepository;
            _tokensRepository = tokensRepository;
        }

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

        public async Task<Tokens> RefreshTokensAsync(TokenValidationParameters tokenValidationParams, string accessToken, string refreshToken, string privateKey)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationResult = await handler.ValidateTokenAsync(accessToken, tokenValidationParams);

            if (!validationResult.IsValid)
                throw new InvalidTokenException();

            var id = (string)validationResult.Claims[ClaimTypes.NameIdentifier];
            Guid userId = Guid.Parse(id);

            var tokens = await _tokensRepository.FindByUserIdAsync(userId);
            var user = await _userRepository.FindById(userId);

            if (tokens?.RefreshToken != refreshToken || user == null)
                throw new InvalidTokenException();

            return GenerateTokens(user, privateKey);
        }
    }
}
