﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

namespace PictureLibrary.API.Controllers
{
    [Route("library")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibraryController(IMediator mediator)
        {
            _mediator = mediator;    
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Library>>> GetAllLibraries([FromQuery] Guid userId)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            var query = new GetUserLibrariesQuery(userId);
            var libraries = await _mediator.Send(query);

            return Ok(libraries);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> AddLibrary([FromBody] NewLibrary library)
        {
            Guid? userId = GetCurrentUserId();
            
            if (!userId.HasValue)
                return Unauthorized();

            var addLibraryCommand = new AddLibraryCommand(library, userId.Value);
            Guid libraryId = await _mediator.Send(addLibraryCommand);

            return Created(string.Empty, libraryId);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateLibrary([FromBody] Library library)
        {
            var findLibraryCommand = new FindLibraryByIdQuery(library.Id);
            var existingLibrary = await _mediator.Send(findLibraryCommand);

            if (existingLibrary == null)
                return BadRequest();

            var updateLibraryCommand = new UpdateLibraryCommand(library);
            await _mediator.Send(updateLibraryCommand);

            return Ok();
        }
    }
}