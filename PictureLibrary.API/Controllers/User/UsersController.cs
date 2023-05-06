using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

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

            IEnumerable<UserDto> userDtos = users.Select(_mapper.Map<UserDto>);

            return Ok(new FindUsersDto()
            { 
                Users = userDtos,
            });
        }

        [Authorize]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserDto updateUserDto, string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                return BadRequest();

            if (!IsUserAuthorized(userId))
                return Unauthorized();

            var user = _mapper.Map<User>(updateUserDto);
            user.Id = userId;

            var command = new UpdateUserCommand(user);
            await _mediator.Send(command);

            return Ok();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                return BadRequest();

            if (!IsUserAuthorized(userId))
                return Unauthorized();

            var query = new GetUserQuery(userId);
            var user = await _mediator.Send(query);

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }
    }
}
