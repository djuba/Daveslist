using Daveslist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Daveslist.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public bool IsActive { get; set; } = true;

        // hardcoding for simplicity, store in a db with hashed passwords in production applications
        [JsonIgnore]
        public static int _nextCategoryId { get; set; } = 1;
        [JsonIgnore]
        public static List<Category> _categories { get; set; } = new List<Category>();

        public Category(CategoryRequest model)
        {
            Id = _nextCategoryId;
            Name = model.Name;
            IsPublic = model.IsPublic;

            _nextCategoryId++;
        }
    }
}
