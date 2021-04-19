using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PictureLibrary_API.Exceptions;
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
                var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullName);
                if(library == null || library.Images.Find(x => x.FullName == imageFile.FullName) == null)
                {
                    return NotFound();
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                image = await ImageRepository.GetBySourceAsync(imageFile.FullName);
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

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<byte[]>>> GetImages([FromQuery] string libraryFullName)
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
                var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullName);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                bool imageExistsInLibrary = library.Images.Remove(library.Images.Find(x => x.FullName == imageFile.FullName));

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
            catch (ContentNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
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
                var library = await LibraryRepository.GetBySourceAsync(image.ImageFile.LibraryFullName);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                bool imageExistsInLibrary = library.Images.Remove(library.Images.Find(x => x.FullName == image.ImageFile.FullName));

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
            catch(ContentNotFoundException e)
            {
                return NotFound(new { message = e.Message });
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
                var library = await LibraryRepository.GetBySourceAsync(image.ImageFile.LibraryFullName);
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
            catch (ContentNotFoundException e)
            {
                return NotFound(new { message = e.Message });
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
                var library = await LibraryRepository.GetBySourceAsync(imageFile.LibraryFullName);
                if (library == null)
                {
                    return BadRequest("Library doesn't exist");
                }

                var userId = User?.Identity.Name;
                if (!library.Owners.Where(x => x.ToString() == userId).Any())
                {
                    return Unauthorized();
                }

                bool imageExistsInLibrary = library.Images.Remove(library.Images.Find(x => x.FullName == imageFile.FullName));

                if(!imageExistsInLibrary)
                {
                    return BadRequest();
                }

                await ImageRepository.RemoveAsync(imageFile.FullName);
                await LibraryRepository.UpdateAsync(library);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (ContentNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet("icons")]
        public async Task<ActionResult<IEnumerable<byte[]>>> GetIcons([FromQuery] string libraryFullName, [FromBody] IEnumerable<string> imageSources)
        {
            var result = new List<byte[]>();

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

                var icons = await ImageRepository.GetIcons(imageSources);

                foreach(var t in icons)
                {
                    using(var ms = new MemoryStream())
                    {
                        var bitmap = t.ToBitmap();
                        bitmap.Save(ms, ImageFormat.Icon);
                        result.Add(ms.ToArray());
                    }
                }
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

            return Ok(result);
        }

        [HttpGet("icon")]
        public async Task<ActionResult<byte[]>> GetIcon([FromQuery] string libraryFullName, [FromQuery] string imageFullName)
        {
            byte[] result;

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

                var icon = await ImageRepository.GetIcon(imageFullName);
                
                using(var ms = new MemoryStream())
                {
                    var bitmap = icon.ToBitmap();
                    bitmap.Save(ms, ImageFormat.Icon);
                    result = ms.ToArray();
                }
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

            return Ok(result);
        }
    }
}