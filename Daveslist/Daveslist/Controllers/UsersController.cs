using Microsoft.AspNetCore.Mvc;
using Daveslist.Models;
using Daveslist.Services;
using Daveslist.Entities;
using System.Linq;

namespace Daveslist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IListingService _listingService;

        public UsersController(IUserService userService, IListingService listingService)
        {
            _userService = userService;
            _listingService = listingService;
        }

        [Authorize(Roles.RegisteredUser)]
        [HttpPost]
        [Route("{userid:int}/messages")]
        public IActionResult PostMessage(int userId, MessageRequest  model)
        {
            var user = _userService.GetById(userId);

            if (user == null)
                return BadRequest(new { message = $"User {userId} does not exist" });

            _userService.AddMessage(user, new Message(model));

            return Ok("Message sent");
        }

        [Authorize(Roles.RegisteredUser)]
        [HttpGet]
        [Route("{userid:int}/messages")]
        public IActionResult GetMessage(int userId)
        {
            var currentUser = (User)HttpContext.Items["User"];

            if (currentUser == null)
                return BadRequest(new { message = "User was not logged in" });

            if (currentUser.Id != userId)
                return BadRequest(new { message = $"User {currentUser.Id} cannot see messages for user {userId}" });

            var user = _userService.GetById(userId);

            if (user == null)
                return BadRequest(new { message = $"User {userId} does not exist" });

            return Ok(user.Messages);
        }

        [HttpGet]
        [Route("{userid:int}/listings")]
        public IActionResult GetListings(int userId)
        {
            var user = _userService.GetById(userId);

            if (user == null)
                return BadRequest(new { message = $"User {userId} does not exist" });

            var listings = _listingService.GetAllByUser(user);
            return Ok(listings);
        }

        [Authorize(Roles.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
