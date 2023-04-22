using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
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

        [HttpGet("all/{libraryId}")]
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

        [HttpGet("file/{imageFileId}")]
        [Authorize]
        public async Task<IActionResult> GetFile(string imageFileId)
        {
            if (!Guid.TryParse(imageFileId, out Guid imageFileIdParsed))
                return BadRequest();

            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();
            
            var query = new GetFileQuery(userId.Value, imageFileIdParsed);

            var response = await _mediator.Send(query);

            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(response.ImageFile.FilePath, out string? contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileStreamResult(response.FileStream, MediaTypeHeaderValue.Parse(contentType));
        }
    }
}
