using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private ILogger<AccessTokenService> Logger { get; }
        private IDatabaseContext Context { get; }
        private AppSettings AppSettings { get; }

        public AccessTokenService(ILogger<AccessTokenService> logger, IDatabaseContext context, IOptions<AppSettings> appSettings)
        {
            Context = context;
            AppSettings = appSettings.Value;
            Logger = logger;
        }

        public async Task DeleteRefreshTokenAsync(string userId, string refreshToken)
        {
            var token = Context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == refreshToken)
                .FirstOrDefault();
            await Task.Run(() => Context.RefreshTokens.Remove(token));
            await Context.SaveChangesAsync();

            Logger.LogInformation("Removed refresh token from database: " + refreshToken);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<string> GetRefreshTokenAsync(string userId)
        {
            var token = await Task.Run(() => Context.RefreshTokens
                .Where(x => x.UserId == userId)
                .FirstOrDefault());
            
            return token.Token;
        }

        public async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var refToken = new RefreshToken();

            refToken.Id = Guid.NewGuid();
            refToken.UserId = userId;
            refToken.Token = refreshToken;

            await Task.Run(() => Context.RefreshTokens.Add(refToken));
            await Context.SaveChangesAsync();

            Logger.LogInformation("Added refresh token to database: " + refreshToken);
        }

        public string GenerateAccessToken(string userId)
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

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
                
            return principal;
        }
    }
}
