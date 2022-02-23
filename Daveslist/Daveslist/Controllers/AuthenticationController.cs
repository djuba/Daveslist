using Microsoft.AspNetCore.Mvc;
using Daveslist.Models;
using Daveslist.Services;
using Daveslist.Entities;

namespace Daveslist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model, Roles role = Roles.RegisteredUser)
        {
            var response = _userService.Register(model, role);

            if (response == null)
                return BadRequest(new { message = "Username already exists" });

            return Ok(response);
        }

        [HttpPost("register-admin")]
        public IActionResult RegisterAdmin(RegisterRequest model)
        {
            return Register(model, Roles.Admin);
        }

        [HttpPost("register-moderator")]
        public IActionResult RegisterModerator(RegisterRequest model)
        {
            return Register(model, Roles.Moderator);
        }
    }
}
