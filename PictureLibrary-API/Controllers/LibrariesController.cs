using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibrary_API.Repositories;

namespace PictureLibrary_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private ILogger<LibrariesController> Logger { get; }
        private ILibraryRepository LibraryRepository { get; }

        public LibrariesController(ILogger<LibrariesController> logger, ILibraryRepository libraryFileService)
        {
            Logger = logger;
            LibraryRepository = libraryFileService;
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<Library>> GetLibrary(string name)
        {
            Library library = null;

            try
            {
                library = await Task.Run(() => LibraryRepository.FindAsync(x => x.Name == name));
                if (library == null)
                {
                    return NotFound();
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }
            catch(Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok(library);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            var allLibraries = await Task.Run(() => LibraryRepository.GetAllAsync());
            var libraries = new List<Library>();

            var userId = User?.Identity.Name;
            if(allLibraries != null)
            {
                foreach(var l in allLibraries)
                {
                    if (l.Owners.Where(x => x.ToString() == userId).Any()) libraries.Add(l);
                }
            }

            return Ok(libraries);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> PutLibrary(string name, [FromBody] Library library)
        {
            if(name != library.Name)
            {
                return BadRequest();
            }

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any())
            {
                return Unauthorized();
            }

            await Task.Run(() => LibraryRepository.UpdateAsync(library));

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Library>> PostLibrary([FromBody] Library library)
        {
            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any())
            {
                return Unauthorized();
            }

            await Task.Run(() => LibraryRepository.AddAsync(library));

            return CreatedAtAction("GetLibrary", new { name = library.Name }, library);
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Library>> DeleteLibrary(string name)
        {
            var library = await Task.Run(() => LibraryRepository.FindAsync(x => x.Name == name));

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any())
            {
                return Unauthorized();
            }

            if (library == null)
            {
                return NotFound();
            }

            await Task.Run(() => LibraryRepository.RemoveAsync(library));

            return Ok(library);
        }
    }
}
