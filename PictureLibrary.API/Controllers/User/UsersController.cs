using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.Model;
using System.Security.Claims;

namespace PictureLibrary.API.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister user)
        {
            AddUserCommand addUserCommand = new(user);
            Guid userId = await _mediator.Send(addUserCommand);

            return Created(string.Empty, userId);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid userId)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            DeleteUserCommand deleteUserCommand = new(username);
            await _mediator.Send(deleteUserCommand);

            return Ok();
        }
    }
}
