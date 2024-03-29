﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

namespace PictureLibrary.API.Controllers
{
    [Route("library")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public LibraryController(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;    
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllLibraries([FromQuery] Guid userId)
        {
            if (!IsUserAuthorized(userId))
                return Unauthorized();

            var query = new GetUserLibrariesQuery(userId);
            var libraries = await _mediator.Send(query);

            var libraryDtos = libraries.Select(_mapper.Map<LibraryDto>);

            return Ok(new { Libraries = libraryDtos });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLibrary([FromBody] LibraryDto libraryDto)
        {
            Guid? userId = GetCurrentUserId();
            
            if (!userId.HasValue)
                return Unauthorized();

            var library = _mapper.Map<LibraryDto, Library>(libraryDto);
            var addLibraryCommand = new AddLibraryCommand(library, userId.Value);
            Guid libraryId = await _mediator.Send(addLibraryCommand);

            return Created(string.Empty, new { Libraryid = libraryId });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateLibrary(string id, [FromBody] LibraryDto libraryDto)
        {
            if (!Guid.TryParse(id, out Guid libraryId))
                return BadRequest();

            var findLibraryCommand = new FindLibraryByIdQuery(libraryId);
            var existingLibrary = await _mediator.Send(findLibraryCommand);

            if (existingLibrary == null)
                return BadRequest();

            var library = _mapper.Map<LibraryDto, Library>(libraryDto);
            var updateLibraryCommand = new UpdateLibraryCommand(library);
            await _mediator.Send(updateLibraryCommand);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLibrary(string id)
        {
            if (!Guid.TryParse(id, out Guid libraryId))
                return BadRequest();

            var command = new DeleteLibraryCommand(libraryId);
            await _mediator.Send(command);

            return Ok();
        }
    }
}
