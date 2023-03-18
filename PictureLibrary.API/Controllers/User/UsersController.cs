﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Api.Dtos;
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
                return NotFound("User not found.");

            return Ok(user);
        }
    }
}
