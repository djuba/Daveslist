using Daveslist.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Daveslist.Entities
{
    public class Listing
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [StringLength(5000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Content { get; set; }

        public Category Category { get; set; }

        public List<Reply> Replies { get; set; }

        public User CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsPublic { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsTrashed { get; set; } = false;

        // hardcoding for simplicity, store in a db with hashed passwords in production applications
        [JsonIgnore]
        public static int _nextListingId { get; set; } = 1;
        [JsonIgnore]
        public static List<Listing> _listings { get; set; } = new List<Listing>();

        public Listing(ListingRequest model, Category category, User user)
        {
            Id = _nextListingId;
            Title = model.Title;
            Content = model.Content;
            Category = category;
            Replies = new List<Reply>();
            IsPublic = model.IsPublic;
            IsTrashed = model.IsTrashed;
            CreatedBy = user;
            CreatedOn = DateTime.Now;

            _nextListingId++;
        }
    }
}
