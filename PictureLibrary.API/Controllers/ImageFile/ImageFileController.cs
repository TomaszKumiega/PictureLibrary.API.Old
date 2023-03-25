using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Queries;

namespace PictureLibrary.API.Controllers.ImageFile
{
    [Route("imageFile")]
    [ApiController]
    public class ImageFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageFileController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("all/{libraryId}")]
        [Authorize]
        public async Task<IActionResult> GetAllImageFiles(string libraryId)
        {
            if (!Guid.TryParse(libraryId, out Guid libraryIdParsed))
                return BadRequest();

            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            var query = new GetImageFilesQuery(libraryIdParsed, userId.Value);
            var imageFiles = await _mediator.Send(query);

            return Ok(new { ImageFiles = imageFiles });
        }
    }
}
