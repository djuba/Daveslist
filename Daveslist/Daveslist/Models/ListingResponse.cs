using Daveslist.Entities;
using System;

namespace Daveslist.Models
{
    public class ListingResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        //public int CategoryId { get; set; }

        public User CreatedBy { get; set; }

        public bool IsPublic { get; set; }

        public bool IsTrashed { get; set; } = false;


        public ListingResponse(Listing listing)
        {
            Id = listing.Id;
            Title = listing.Title;
            Content = listing.Content;
            CreatedBy = listing.CreatedBy;
            IsPublic = listing.IsPublic;
            IsTrashed = listing.IsTrashed;
        }
    }
}