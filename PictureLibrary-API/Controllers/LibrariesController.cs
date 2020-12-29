using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Repositories;
using PictureLibraryModel.Model;

namespace PictureLibrary_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private readonly ILogger<LibrariesController> _logger;
        private IRepository<Library> _libraryRepository;

        public LibrariesController(ILogger<LibrariesController> logger, IRepository<Library> libraryFileService)
        {
            _logger = logger;
            _libraryRepository = libraryFileService;
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<Library>> GetLibrary(string name)
        {
            var library = await _libraryRepository.FindLibrary(name);

            if (library == null)
            {
                return NotFound();
            }

            return Ok(library);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            var libraries = await _libraryRepository.GetAllLibrariesAsync();

            return Ok(libraries);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> PutLibrary(string name, [FromBody] Library library)
        {
            if(name != library.Name)
            {
                return BadRequest();
            }

            try
            {
                await _libraryRepository.UpdateLibrary(library);
            }
            catch(Exception e)
            {
                //TODO: add more exception cases
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Library>> PostLibrary([FromBody] Library library)
        {
            try
            {
                await _libraryRepository.AddLibrary(library);
            }
            catch (Exception e)
            {
                //TODO: add more exception cases
                throw;
            }

            return CreatedAtAction("GetLibrary", new { name = library.Name }, library);
        }

        [HttpDelete("name")]
        public async Task<ActionResult<Library>> DeleteLibrary(string name)
        {
            var library = await _libraryRepository.FindLibrary(name);

            if(library == null)
            {
                return NotFound();
            }

            await _libraryRepository.DeleteLibrary(library);

            return library;
        }
    }
}
