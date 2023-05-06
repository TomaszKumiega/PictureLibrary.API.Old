using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;

namespace PictureLibrary.API.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            AddUserCommand addUserCommand = _mapper.Map<AddUserCommand>(user);
            Guid userId = await _mediator.Send(addUserCommand);

            return Created(string.Empty, new { UserId = userId });
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
        public async Task<IActionResult> FindUsers(string username)
        {
            FindUsersQuery findUsersQuery = new(username);
            var users = await _mediator.Send(findUsersQuery);

            IEnumerable<GetUserDto> userDtos = users.Select(_mapper.Map<GetUserDto>);

            return Ok(userDtos);
        }
    }
}
