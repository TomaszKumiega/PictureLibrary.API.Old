using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Tools.ContentRangeValidator;

namespace PictureLibrary.API.Controllers.ImageFile
{
    [Route("imageFile")]
    [ApiController]
    public class ImageFileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IContentRangeValidator _contentRangeValidator;

        public ImageFileController(
            IMapper mapper,
            IMediator mediator,
            IContentRangeValidator contentRangeValidator)
        {
            _mapper = mapper;
            _mediator = mediator;
            _contentRangeValidator = contentRangeValidator;
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
            var imageFileDtos = imageFiles.Select(_mapper.Map<ImageFileDto>);
            
            return Ok(new { ImageFiles = imageFileDtos });
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

        [HttpPost("createUploadSession")]
        [Authorize]
        public async Task<IActionResult> CreateUploadSession([FromBody] CreateUploadSessionDto createUploadSessionDto)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            if (!Request.Headers.TryGetValue("Content-Range", out var contentRange))
            {
                return BadRequest("Request doesn't contain content range header.");
            }

            var command = new CreateUploadSessionCommand(userId.Value, createUploadSessionDto.FileName, contentRange!, createUploadSessionDto.Libraries);

            var uploadSessionId = await _mediator.Send(command);

            return Ok(new { UploadUrl = $"{Url}/uploadFile/{uploadSessionId}" });
        }

        [HttpPost("uploadFile/{uploadSessionId}")]
        [Authorize]
        public async Task<IActionResult> UploadFile(string uploadSessionId)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            if (!Guid.TryParse(uploadSessionId, out Guid uploadSessionIdParsed))
                return BadRequest();

            if (!Request.Headers.TryGetValue("Content-Range", out var contentRange))
                return BadRequest("Request doesn't contain content range header.");

            if (!_contentRangeValidator.IsContentRangeValid(contentRange.ToString()))
                return BadRequest("Content-Range header is invalid.");

            long contentLength = Request.ContentLength!.Value;
            byte[] buffer = new byte[contentLength];
            int bytesRead = await Request.Body.ReadAsync(buffer.AsMemory(0, (int)contentLength));

            var command = new UploadFileCommand(userId.Value, uploadSessionIdParsed, buffer, contentRange.ToString(), bytesRead);

            var imageFile = await _mediator.Send(command);

            if (imageFile != null)
            {
                return Ok(_mapper.Map<ImageFileDto>(imageFile));
            }
            
            return StatusCode(100);
        }

        [HttpDelete("{imageFileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteImageFile(string imageFileId)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            if(!Guid.TryParse(imageFileId, out Guid id)) 
            {
                return BadRequest();
            }

            var command = new DeleteImageFileCommand(userId.Value, id);

            await _mediator.Send(command);

            return Ok();
        }

        [HttpPatch("{imageFileId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImageFile(string imageFileId, [FromBody] UpdateImageFileDto updateImageFileDto)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            if (!Guid.TryParse(imageFileId, out Guid id))
            {
                return BadRequest();
            }

            if (updateImageFileDto.Name == null)
            {
                return BadRequest();
            }

            var command = new UpdateImageFileCommand(userId.Value, id, updateImageFileDto.Name);
            await _mediator.Send(command);

            return Ok();
        }
    }
}
