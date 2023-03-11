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
    public class UsersController : Controller
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
            await _mediator.Send(addUserCommand);

            return Created(string.Empty, addUserCommand);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username must not be empty");
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) 
            {
                return Unauthorized();
            }

            Guid userGuid = Guid.Parse(userId);
            DeleteUserCommand deleteUserCommand = new(userGuid, username);
            await _mediator.Send(deleteUserCommand);

            return Ok();
        }
    }
}
