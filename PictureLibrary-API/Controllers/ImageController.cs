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
    }
}