using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibrary_API.Repositories;

namespace PictureLibrary_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private IImageRepository _imageRepository;

        public ImageController(ILogger<ImageController> logger, IImageRepository imageRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
        }

        [HttpGet("{source}")]
        public async Task<ActionResult<byte[]>> GetImage(string source)
        {
            var image = await _imageRepository.GetBySourceAsync(source);

            if(image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<byte[]>>> GetImages()
        {
            var images = await _imageRepository.GetAllAsync();

            if(images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        [HttpPut("{source}")]
        public async Task<IActionResult> PutImage(string source, [FromBody] ImageFile imageFile)
        {
            if(source != imageFile.Source)
            {
                return BadRequest();
            }

            try
            {
                await _imageRepository.UpdateAsync(imageFile);
            }
            catch(Exception e)
            {
                //TODO
                throw;
            }

            return NoContent();
        }

        [HttpPost("{name}")]
        public async Task<ActionResult<ImageFile>> PostImage(string name, [FromBody] byte[] image)
        {
            try
            {
                await _imageRepository.AddAsync(image);
            }
            catch(Exception e)
            {
                //TODO
                throw;
            }

            return CreatedAtAction("GetImage", new { name = name }, image);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteImage(string source)
        {
            try
            {
                await _imageRepository.RemoveAsync(source);
            }
            catch(Exception e)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}