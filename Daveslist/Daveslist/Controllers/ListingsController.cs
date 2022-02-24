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
        private ICategoryService _categoryService;

        public ListingsController(IListingService listingService, ICategoryService categoryService)
        {
            _listingService = listingService;
            _categoryService = categoryService;
        }

        [Authorize(Roles.Admin, Roles.RegisteredUser)]
        [HttpPost]
        public IActionResult PostListing(ListingRequest model)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var category = _categoryService.GetById(model?.CategoryId ?? 0);

            var response = _listingService.PostListing(model, category, user);

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
        public IActionResult PatchListing(int listingId, ListingRequest model)
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

        [Authorize(Roles.RegisteredUser)]
        [HttpPost]
        [Route("{listingId:int}/replies")]
        public IActionResult PostReply(int listingId, ReplyRequest model)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var response = _listingService.PostReply(listingId, model, user);

            if (response == null)
                return BadRequest(new { message = $"User {user.Id} cannot reply to listing {listingId}" });

            return Ok(response);
        }

        [Authorize(Roles.Admin, Roles.Moderator, Roles.RegisteredUser)]
        [HttpGet]
        [Route("{listingId:int}/replies")]
        public IActionResult GetAllReplies(int listingId)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var replies = _listingService.GetAllReplies(listingId, user);

            return Ok(replies);
        }
    }
}
