using Daveslist.Entities;
using System;
using System.Collections.Generic;

namespace Daveslist.Models
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }


        public CategoryResponse(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            IsPublic = category.IsPublic;
        }
    }
}