using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

namespace PictureLibrary.API.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public LoginController(
            IMediator mediator,
            IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin.Username == null)
                return BadRequest();

            FindUserQuery query = new(userLogin.Username);
            var user = await _mediator.Send(query);

            if (user != null)
            {
                string token = "token"; // generate token
                return Ok(token);
            }

            return NotFound("User not found");
        }
    }
}
