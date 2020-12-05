using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibraryModel.Model;
using PictureLibraryModel.Services;

namespace PictureLibrary_RaspberryAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private readonly ILogger<LibrariesController> _logger;
        private ILibraryFileService _libraryFileService;

        public LibrariesController(ILogger<LibrariesController> logger, ILibraryFileService libraryFileService)
        {
            _logger = logger;
            _libraryFileService = libraryFileService;
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<Library>> GetLibrary(string name)
        {
            var library = await _libraryFileService.FindLibrary(name);

            if (library == null)
            {
                return NotFound();
            }

            return Ok(library);
        }
    }
}
