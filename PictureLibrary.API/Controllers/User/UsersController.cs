using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

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
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                return BadRequest();

            if (!IsUserAuthorized(userId))
                return Unauthorized();

            DeleteUserCommand deleteUserCommand = new(userId);
            await _mediator.Send(deleteUserCommand);

            return Ok();
        }

        [Authorize]
        [HttpGet("find/{username}")]
        public async Task<IActionResult> FindUser(string username)
        {
            FindUserQuery findUserQuery = new(username);
            var user = await _mediator.Send(findUserQuery);

            if (user == null || !IsUserAuthorized(user.Id))
                return NotFound();

            return Ok(user);
        }
    }
}
