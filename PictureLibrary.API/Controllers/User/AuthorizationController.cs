using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;

namespace PictureLibrary.API.Controllers
{
    [Route("authorization")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public AuthorizationController(
            IMapper mapper,
            IMediator mediator,
            IConfiguration config)
        {
            _mapper = mapper;
            _config = config;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            if (userLogin.Username == null || userLogin.Password == null)
                return BadRequest();

            AuthorizeUserQuery authorizeUserQuery = new(userLogin.Username, userLogin.Password!, _config["PrivateKey"]!);
            var tokens = await _mediator.Send(authorizeUserQuery);

            if (tokens != null)
            {
                SaveTokensCommand saveTokensCommand = new(tokens);
                await _mediator.Send(saveTokensCommand);

                return Ok(tokens);
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensDto refreshTokensDto)
        {
            var query = new RefreshTokensQuery(new PictureLibraryTokenValidationParameters(_config), refreshTokensDto.AccessToken, refreshTokensDto.RefreshToken, _config["PrivateKey"]);
            var tokens = await _mediator.Send(query);

            if (tokens == null)
                return BadRequest();

            return Ok(tokens);
        }
    }
}
