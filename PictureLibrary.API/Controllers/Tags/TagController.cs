using MediatR;
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

            var command = new AddTagCommand(userId.Value, tagDto.Name, tagDto.Description, tagDto.Description, tagDto.Libraries);
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


            var tagDtos = tags.Select(x => new GetTagDto()
            {
                Id = x.Id,
                Name = x.Name,
                ColorHex = x.ColorHex,
                Description = x.Description,
                Libraries = x.Libraries?.Select(x => x.Id) ?? Enumerable.Empty<Guid>(),
            });

            return Ok(tagDtos);
        }
    }
}
