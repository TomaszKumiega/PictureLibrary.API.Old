using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibrary_API.Repositories;

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
            var library = await Task.Run(() => _libraryRepository.Find(x => x.Name == name));

            if (library == null)
            {
                return NotFound();
            }

            return Ok(library);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            var libraries = await Task.Run(() => _libraryRepository.GetAll());

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
                await Task.Run(() => _libraryRepository.Update(library));
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
                await Task.Run(() => _libraryRepository.Add(library));
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
            var library = await Task.Run(() => _libraryRepository.Find(x => x.Name == name));

            if(library == null)
            {
                return NotFound();
            }

            await Task.Run(() => _libraryRepository.Remove(library));

            return library;
        }
    }
}
