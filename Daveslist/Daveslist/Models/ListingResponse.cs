using Daveslist.Entities;
using System;
using System.Collections.Generic;

namespace Daveslist.Models
{
    public class ListingResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        //public int CategoryId { get; set; }

        public User CreatedBy { get; set; }

        public List<Reply> Replies { get; set; }

        public bool IsPublic { get; set; }

        public bool IsTrashed { get; set; } = false;


        public ListingResponse(Listing listing)
        {
            Id = listing.Id;
            Title = listing.Title;
            Content = listing.Content;
            CreatedBy = listing.CreatedBy;
            Replies = listing.Replies;
            IsPublic = listing.IsPublic;
            IsTrashed = listing.IsTrashed;
        }
    }
}