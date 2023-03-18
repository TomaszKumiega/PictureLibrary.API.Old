using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;

namespace PictureLibrary.API.Controllers
{
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TagController(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTag([FromBody] TagDto tagDto)
        {
            Guid? userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();

            var command = new AddTagCommand(userId.Value, tagDto.Name, tagDto.Description, tagDto.Description, tagDto.Libraries);
            var tagId = await _mediator.Send(command);

            return Created(string.Empty, new { TagId = tagId });
        }
    }
}
