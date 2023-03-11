using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

namespace PictureLibrary.API.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAccessTokenService _accessTokenService;

        public LoginController(
            IMediator mediator,
            IAccessTokenService accessTokenService)
        {
            _mediator = mediator;
            _accessTokenService = accessTokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin.Username == null)
                return BadRequest();

            FindUserQuery findUserQuery = new(userLogin.Username);
            var user = await _mediator.Send(findUserQuery);

            if (user != null)
            {
                Tokens tokens = _accessTokenService.GenerateTokens(user);

                SaveTokensCommand saveTokensCommand = new(tokens);
                await _mediator.Send(saveTokensCommand);

                return Ok(tokens);
            }

            return NotFound("User not found");
        }
    }
}
