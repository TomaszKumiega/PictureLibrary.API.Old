﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.Model;

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
            if (user.EmailAddress == null || user.Password == null || user.EmailAddress == null)
            {
                return BadRequest();
            }

            AddUserCommand addUserCommand = new(user);
            await _mediator.Send(addUserCommand);

            return Created(string.Empty, addUserCommand);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string username)
        {
            DeleteUserCommand deleteUserCommand = new(username);
            await _mediator.Send(deleteUserCommand);

            return Ok();
        }
    }
}
