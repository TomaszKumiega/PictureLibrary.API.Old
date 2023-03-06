using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;
using System.Security.Claims;

namespace PictureLibrary.API.Controllers
{
    [Route("library")]
    [ApiController]
    public class LibraryController : Controller
    {
        private readonly IMediator _mediator;

        public LibraryController(
            IMediator mediator
            )
        {
            _mediator = mediator;    
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Library>>> GetAllLibraries([FromBody] Guid userId)
        {
             var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(id, out Guid currentUserId) || currentUserId != userId)
            {
                return Unauthorized();
            }

            var query = new GetUserLibrariesQuery(userId);
            var libraries = await _mediator.Send(query);

            return Ok(libraries);
        }
    }
}
