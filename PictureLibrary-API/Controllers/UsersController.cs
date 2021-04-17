using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PictureLibrary_API.Helpers;
using PictureLibrary_API.Model;
using PictureLibrary_API.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PictureLibrary_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ILogger<UsersController> Logger { get; }
        private IUserService UserService { get; }
        private IMapper Mapper { get; }
        private IRefreshTokenService RefreshTokenService { get; }
        private AppSettings AppSettings { get; }

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IOptions<AppSettings> appSettings, IUserService userService, IRefreshTokenService refreshTokenService)
        {
            Logger = logger;
            Mapper = mapper;
            AppSettings = appSettings.Value;
            UserService = userService;
            RefreshTokenService = refreshTokenService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            User user = null;
            string tokenString;
            string refreshToken;

            try
            {
                user = UserService.Authenticate(model.Username, model.Password);

                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                tokenString = GenerateToken(user.Id.ToString());
                refreshToken = RefreshTokenService.GenerateToken();
                RefreshTokenService.SaveRefreshToken(user.Id.ToString(), refreshToken);
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }
            
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = tokenString,
                RefreshToken = refreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel model)
        {
            var user = Mapper.Map<User>(model);

            try
            {
                UserService.Create(user, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userId = principal.Identity.Name;
            var savedRefreshToken = RefreshTokenService.GetRefreshToken(userId); 
            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            string newJwtToken = null;
            string newRefreshToken = null;

            try
            {
                newJwtToken = GenerateToken(userId);
                newRefreshToken = RefreshTokenService.GenerateToken();

                RefreshTokenService.DeleteRefreshToken(userId, refreshToken);
                RefreshTokenService.SaveRefreshToken(userId, newRefreshToken);
            }
            catch(Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var userId = User?.Identity.Name;
            if (userId != id.ToString())
            {
                return Unauthorized();
            }

            User user = null;
            
            try
            {
                user = UserService.GetById(id);
            }
            catch(Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]UserModel model)
        {
            var userId = User?.Identity.Name;
            if (userId != id.ToString())
            {
                return Unauthorized();
            }

            var user = Mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // fix exceptions
                UserService.Update(user, model.Password);
            }
            catch(Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var userId = User?.Identity.Name;
            if (userId != id.ToString())
            {
                return Unauthorized();
            }

            UserService.Delete(id);

            return Ok();
        }

        private string GenerateToken(string userId)
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

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

    }
}