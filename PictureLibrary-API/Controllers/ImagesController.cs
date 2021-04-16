using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Model;
using PictureLibrary_API.Repositories;
using Image = PictureLibrary_API.Model.Image;

namespace PictureLibrary_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private ILogger<ImagesController> Logger { get; }
        private IImageRepository ImageRepository { get; }
        private ILibraryRepository LibraryRepository { get; }

        public ImagesController(ILogger<ImagesController> logger, IImageRepository imageRepository, ILibraryRepository libraryRepository)
        {
            Logger = logger;
            ImageRepository = imageRepository;
            LibraryRepository = libraryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<byte[]>> GetImage([FromBody] ImageFile imageFile)
        {
            // check if library exists
            var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullPath);
            if (library == null || library.Images.Find(x => x.FullPath == imageFile.FullPath) == null)
            {
                return BadRequest();
            }

            // check if user owns the library
            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any())
            {
                return Unauthorized();
            }

            var image = await ImageRepository.GetBySourceAsync(imageFile.FullPath);

            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpGet("{libraryFullName}")]
        public async Task<ActionResult<IEnumerable<byte[]>>> GetImages(string libraryFullName)
        {
            // check if library exists
            var library = await LibraryRepository.GetBySourceAsync(libraryFullName);
            if (library == null)
            {
                return BadRequest();
            }

            // check if user owns the library
            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any())
            {
                return Unauthorized();
            }

            var images = await ImageRepository.GetAllAsync(libraryFullName);

            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        [HttpPut("imageFile")]
        public async Task<IActionResult> PutImage([FromBody] ImageFile imageFile)
        {
            // check if library exists
            var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullPath);
            if (library == null)
            {
                return BadRequest();
            }

            // check if user owns the library
            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any())
            {
                return Unauthorized();
            }

            var updatedImage = await ImageRepository.UpdateAsync(imageFile);

            library.Images.Remove(library.Images.Find(x => x.FullPath == imageFile.FullPath));
            library.Images.Add(updatedImage);

            await LibraryRepository.UpdateAsync(library);

            return NoContent();
        }

        [HttpPut("imageContent")]
        public async Task<IActionResult> PutImage([FromBody] Image image)
        {
            var library = await LibraryRepository.GetBySourceAsync(image.ImageFile.LibraryFullPath);

            if (library == null) return BadRequest();

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any()) return Unauthorized();

            var updatedImage = await ImageRepository.UpdateAsync(image);

            library.Images.Remove(library.Images.Find(x => x.FullPath == image.ImageFile.FullPath));
            library.Images.Add(updatedImage);

            await LibraryRepository.UpdateAsync(library);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ImageFile>> PostImage([FromBody] Image image)
        {
            var library = await LibraryRepository.GetBySourceAsync(image.ImageFile.LibraryFullPath);

            if (library == null) return BadRequest("Library doesn't exist");

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any()) return Unauthorized();

            var imageFile = await ImageRepository.AddAsync(image);

            library.Images.Add(imageFile);
            await LibraryRepository.UpdateAsync(library);

            return CreatedAtAction("GetImage", new { name = imageFile.Name }, imageFile);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromBody] ImageFile imageFile)
        {
            var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullPath);

            if (library == null) return BadRequest("Library doesn't exist");

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any()) return Unauthorized();

            try
            {
                await ImageRepository.RemoveAsync(imageFile.FullPath);
            }
            catch
            {
                return NotFound();
            }

            library.Images.Remove(library.Images.Find(x => x.FullPath == imageFile.FullPath));
            await LibraryRepository.UpdateAsync(library);

            return Ok();
        }

        [HttpGet("icons/{librarySource}")]
        public async Task<ActionResult<IEnumerable<Icon>>> GetIcons(string librarySource, [FromBody] IEnumerable<string> imageSources)
        {
            var library = await LibraryRepository.GetBySourceAsync(librarySource);

            if (library == null) return BadRequest("Library doesn't exist");

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any()) return Unauthorized();

            var icons = await ImageRepository.GetIcons(imageSources);

            return Ok(icons);
        }

        [HttpGet("icons/{librarySource}/{imageSource}")]
        public async Task<IActionResult> GetIcon(string librarySource, string imageSource)
        {
            var library = await LibraryRepository.GetBySourceAsync(librarySource);

            if (library == null) return BadRequest("Library doesn't exist");

            var userId = User?.Identity.Name;
            if (!library.Owners.Where(x => x.ToString() == userId).Any()) return Unauthorized();

            var icon = await ImageRepository.GetIcon(imageSource);

            return Ok(icon);
        }
    }
}