﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;

namespace PictureLibrary.API.Controllers
{
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTag([FromBody] AddTagDto tagDto)
        {
            Guid? userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            var command = new AddTagCommand(userId.Value, tagDto.Name, tagDto.Description, tagDto.ColorHex, tagDto.Libraries);
            var tagId = await _mediator.Send(command);

            return Created(string.Empty, new { TagId = tagId });
        }

        [HttpGet("{libraryId}")]
        [Authorize]
        public async Task<IActionResult> GetTags(string libraryId)
        {
            Guid? userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            if (!Guid.TryParse(libraryId, out Guid libraryIdParsed))
                return BadRequest();

            var query = new GetTagsQuery(userId.Value, libraryIdParsed);
            var tags = await _mediator.Send(query);


            var tagDtos = tags?.Select(x => new GetTagDto()
            {
                Id = x.Id,
                Name = x.Name,
                ColorHex = x.ColorHex,
                Description = x.Description,
                Libraries = x.Libraries?.Select(x => x.Id) ?? Enumerable.Empty<Guid>(),
            }) ?? Enumerable.Empty<GetTagDto>();

            return Ok(tagDtos);
        }

        [HttpDelete()]
        [Authorize]
        public async Task<IActionResult> DeleteTag([FromQuery] string libraryId, [FromQuery] string tagId)
        {
            if (!Guid.TryParse(libraryId, out Guid libraryIdParsed))
                return BadRequest();
            if (!Guid.TryParse(tagId, out Guid tagIdParsed))
                return BadRequest();

            var command = new DeleteTagCommand(libraryIdParsed, tagIdParsed);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTag(string id, [FromBody] UpdateTagDto updateTagDto)
        {
            Guid? userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();

            if (!Guid.TryParse(id, out Guid tagId))
                return BadRequest();

            var command = new UpdateTagCommand(userId.Value, tagId, updateTagDto.Name, updateTagDto.Description, updateTagDto.ColorHex, updateTagDto.Libraries);
            await _mediator.Send(command);

            return Ok();
        }
    }
}
