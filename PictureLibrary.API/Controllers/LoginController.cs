using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Model;

namespace PictureLibrary.API.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = new User(); // download user from database

            if (user != null)
            {
                string token = "token"; // generate token
                return Ok(token);
            }

            return NotFound("User not found");
        }
    }
}
