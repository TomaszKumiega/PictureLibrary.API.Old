using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PictureLibrary.API
{
    public class PictureLibraryTokenValidationParameters : TokenValidationParameters
    {
        public PictureLibraryTokenValidationParameters(IConfiguration configuration)
        {
            ValidateIssuer = true;
            ValidateAudience = true;
            ValidateLifetime = true;
            ValidateIssuerSigningKey = true;
            ValidIssuer = configuration["Jwt:Issuer"];
            ValidAudience = configuration["Jwt:Audience"];
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["PrivateKey"]!));
        }
    }
}
