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
            byte[] image = null;

            try
            {
                var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullPath);
                if(library == null || library.Images.Find(x => x.FullPath == imageFile.FullPath) == null)
                {
                    return NotFound();
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                image = await ImageRepository.GetBySourceAsync(imageFile.FullPath);
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
            
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpGet("{libraryFullName}")]
        public async Task<ActionResult<IEnumerable<byte[]>>> GetImages(string libraryFullName)
        {
            IEnumerable<byte[]> images = null;

            try
            {
                var library = await LibraryRepository.GetBySourceAsync(libraryFullName);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                images = await ImageRepository.GetAllAsync(libraryFullName);
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

            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        [HttpPut("imageFile")]
        public async Task<IActionResult> PutImage([FromBody] ImageFile imageFile)
        {
            try
            {
                var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullPath);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                bool imageExistsInLibrary = library.Images.Remove(library.Images.Find(x => x.FullPath == imageFile.FullPath));

                if(!imageExistsInLibrary)
                {
                    return BadRequest();
                }

                var updatedImage = await ImageRepository.UpdateAsync(imageFile);
                library.Images.Add(updatedImage);

                await LibraryRepository.UpdateAsync(library);
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
           
            return NoContent();
        }

        [HttpPut("image")]
        public async Task<IActionResult> PutImage([FromBody] Image image)
        {
            try
            {
                var library = await LibraryRepository.GetBySourceAsync(image.ImageFile.LibraryFullPath);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                bool imageExistsInLibrary = library.Images.Remove(library.Images.Find(x => x.FullPath == image.ImageFile.FullPath));

                if (!imageExistsInLibrary)
                {
                    return BadRequest();
                }

                var updatedImage = await ImageRepository.UpdateAsync(image);
                library.Images.Add(updatedImage);

                await LibraryRepository.UpdateAsync(library);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ImageFile>> PostImage([FromBody] Image image)
        {
            ImageFile imageFile = null;

            try
            {
                var library = await LibraryRepository.GetBySourceAsync(image.ImageFile.LibraryFullPath);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                imageFile = await ImageRepository.AddAsync(image);

                library.Images.Add(imageFile);
                await LibraryRepository.UpdateAsync(library);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return CreatedAtAction("GetImage", new { name = imageFile.Name }, imageFile);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromBody] ImageFile imageFile)
        {
            try
            {
                var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullPath);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                bool imageExistsInLibrary = library.Images.Remove(library.Images.Find(x => x.FullPath == imageFile.FullPath));

                if(!imageExistsInLibrary)
                {
                    return BadRequest();
                }

                await ImageRepository.RemoveAsync(imageFile.FullPath);
                await LibraryRepository.UpdateAsync(library);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet("icons/{libraryFullName}")]
        public async Task<ActionResult<IEnumerable<Icon>>> GetIcons(string libraryFullName, [FromBody] IEnumerable<string> imageSources)
        {
            IEnumerable<Icon> icons = null;

            try
            {
                var library = await LibraryRepository.GetBySourceAsync(libraryFullName);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                icons = await ImageRepository.GetIcons(imageSources);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok(icons);
        }

        [HttpGet("icons/{libraryFullName}/{imageFullName}")]
        public async Task<ActionResult<Icon>> GetIcon(string libraryFullName, string imageFullName)
        {
            Icon icon = null;

            try
            {
                var library = await LibraryRepository.GetBySourceAsync(libraryFullName);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                icon = await ImageRepository.GetIcon(imageFullName);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok(icon);
        }
    }
}