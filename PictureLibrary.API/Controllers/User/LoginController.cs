using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;

namespace PictureLibrary.API.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public LoginController( 
            IMediator mediator,
            IConfiguration config)
        {
            _config = config;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            if (userLogin.Username == null || userLogin.Password == null)
                return BadRequest();

            AuthorizeUserQuery authorizeUserQuery = new AuthorizeUserQuery(userLogin.Username, userLogin.Password!, _config["PrivateKey"]!);
            var tokens = await _mediator.Send(authorizeUserQuery);

            if (tokens != null)
            {
                SaveTokensCommand saveTokensCommand = new(tokens);
                await _mediator.Send(saveTokensCommand);

                return Ok(tokens);
            }

            return NotFound("User not found");
        }
    }
}
