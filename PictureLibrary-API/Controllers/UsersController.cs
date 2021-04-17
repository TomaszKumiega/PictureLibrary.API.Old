using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PictureLibrary_API.Exceptions;
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
        private IAccessTokenService AccessTokenService { get; }

        public UsersController(ILogger<UsersController> logger, IMapper mapper, IUserService userService, IAccessTokenService refreshTokenService)
        {
            Logger = logger;
            Mapper = mapper;
            UserService = userService;
            AccessTokenService = refreshTokenService;
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

                tokenString = AccessTokenService.GenerateAccessToken(user.Id.ToString());
                refreshToken = AccessTokenService.GenerateRefreshToken();
                AccessTokenService.SaveRefreshToken(user.Id.ToString(), refreshToken);
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
            catch (ArgumentException e)
            {
                return BadRequest(new { message = e.Message})
            }
            catch (UserAlreadyExistsException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh(string token, string refreshToken)
        {
            var principal = AccessTokenService.GetPrincipalFromExpiredToken(token);
            var userId = principal.Identity.Name;
            var savedRefreshToken = AccessTokenService.GetRefreshToken(userId); 
            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            string newJwtToken = null;
            string newRefreshToken = null;

            try
            {
                newJwtToken = AccessTokenService.GenerateAccessToken(userId);
                newRefreshToken = AccessTokenService.GenerateRefreshToken();

                AccessTokenService.DeleteRefreshToken(userId, refreshToken);
                AccessTokenService.SaveRefreshToken(userId, newRefreshToken);
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

            try
            {
                UserService.Delete(id);
            }
            catch(Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}