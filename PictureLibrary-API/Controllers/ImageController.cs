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
        private IRepository<Dictionary<ImageFile, byte[]>> _imageRepository;

        public ImageController(ILogger<ImageController> logger, IRepository<Dictionary<ImageFile,byte[]>> imageRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
        }
    }
}