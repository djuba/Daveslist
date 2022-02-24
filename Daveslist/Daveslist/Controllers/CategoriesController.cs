using Microsoft.AspNetCore.Mvc;
using Daveslist.Models;
using Daveslist.Services;
using Daveslist.Entities;
using System.Linq;

namespace Daveslist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        private IUserService _userService;
        private IListingService _listingService;

        public CategoriesController(ICategoryService categoryService, IUserService userService, IListingService listingService)
        {
            _categoryService = categoryService;
            _userService = userService;
            _listingService = listingService;
        }

        [Authorize(Roles.Admin, Roles.Moderator)]
        [HttpPost]
        public IActionResult PostCategory(CategoryRequest model)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var response = _categoryService.PostCategory(model);

            if (response == null)
                return BadRequest(new { message = "Listing could not be created, see response for details" });

            return Ok(response);
        }

        [Authorize(Roles.Admin, Roles.Moderator)]
        [HttpDelete]
        [Route("{categoryId}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var user = (User)HttpContext.Items["User"];
            if (user == null)
                return BadRequest(new { message = "User was not logged in" });

            var response = _categoryService.DeleteCategory(categoryId, _listingService);

            if (response == null)
                return BadRequest(new { message = $"Category {categoryId} could not be deleted by user {user.Id}" });

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = (User)HttpContext.Items["User"] ?? null;
            var categories = _categoryService.GetAll(user);
            return Ok(categories);
        }
    }
}
