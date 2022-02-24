using Microsoft.AspNetCore.Mvc;
using Daveslist.Models;
using Daveslist.Services;
using Daveslist.Entities;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;

namespace Daveslist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingsController : ControllerBase
    {
        private IListingService _listingService;
        private IUserService _userService;

        public ListingsController(IListingService listingService,IUserService userService)
        {
            _listingService = listingService;
            _userService = userService;
        }

        [Authorize(Roles.Admin, Roles.RegisteredUser)]
        [HttpPost]
        public IActionResult PostListing(ListingRequest model)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var response = _listingService.PostListing(model, user);

            if (response == null)
                return BadRequest(new { message = "Listing could not be created, see response for details" });

            return Ok(response);
        }

        [Authorize(Roles.Admin, Roles.RegisteredUser)]
        [HttpDelete]
        [Route("{listingId}")]
        public IActionResult DeleteListing(int listingId)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var response = _listingService.DeleteListing(listingId, user);

            if (response == null)
                return BadRequest(new { message = $"Listing {listingId} could not be deleted by user {user.Id}" });

            return Ok(response);
        }

        [Authorize(Roles.Admin, Roles.RegisteredUser)]
        [HttpPatch]
        [Route("{listingId:int}")]
        public IActionResult PatchListing(int listingId, [FromBody] ListingRequest model)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var response = _listingService.PatchListing(listingId, model, user);

            if (response == null)
                return BadRequest(new { message = $"Listing {listingId} could not be updated by user {user.Id}" });

            return Ok(response);
        }

        [Authorize(Roles.Moderator)]
        [HttpPatch]
        [Route("~/moderator/listings/{listingId:int}")]
        public IActionResult PatchListing(int listingId, bool isTrashed)
        {
            var user = (User)HttpContext.Items["User"];
            var response = _listingService.UpdateIsTrashed(listingId, isTrashed);

            if (response == null)
                return BadRequest(new { message = $"Listing {listingId} could not be updated by user {user?.Id}" });

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = (User)HttpContext.Items["User"] ?? null;
            var listings = _listingService.GetAll(user);
            return Ok(listings);
        }
    }
}
