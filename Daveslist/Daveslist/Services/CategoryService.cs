using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Daveslist.Entities;
using Daveslist.Helpers;
using Daveslist.Models;

namespace Daveslist.Services
{
    public interface ICategoryService
    {
        CategoryResponse PostCategory(CategoryRequest model);
        CategoryResponse DeleteCategory(int categoryId, IListingService listingService);
        IEnumerable<Category> GetAll(User user);
        Category GetById(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly AppSettings _appSettings;

        public CategoryService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public CategoryResponse PostCategory(CategoryRequest model)
        {
            var category = new Category(model);
            Category._categories.Add(category);

            return new CategoryResponse(category);
        }

        public CategoryResponse DeleteCategory(int categoryId, IListingService listingService)
        {
            var category = GetById(categoryId);

            if (category == null)
                return null;

            category.IsActive = false;

            // trash all associated listings and clear out their category
            var listings = listingService.GetAllByCategory(category);
            foreach(var listing in listings)
            {
                listing.IsTrashed = true;
                listing.Category = null;
            }

            return new CategoryResponse(category);
        }

        public IEnumerable<Category> GetAll(User user)
        {
            var categories = Category._categories.Where(x => x.IsActive);

            if ((user?.MaxRole ?? Roles.Visitor) == Roles.Visitor)
            {
                return categories.Where(x => x.IsPublic);
            }
            
            return categories;
        }

        public Category GetById(int id)
        {
            return Category._categories.FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}