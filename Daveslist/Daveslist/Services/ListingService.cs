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
    public interface IListingService
    {
        ListingResponse PostListing(ListingRequest model, Category category, User user);
        ListingResponse DeleteListing(int listingId, User user);
        ListingResponse PatchListing(int listingId, ListingRequest listingRequest, User user);
        ListingResponse UpdateIsTrashed(int listingId, bool isTrashed);
        IEnumerable<Listing> GetAll(User user);
        IEnumerable<Listing> GetAllByUser(User user);
        Listing GetById(int id);
        ListingResponse PostReply(int listingId, ReplyRequest model, User user);
        IEnumerable<Reply> GetAllReplies(int listingId, User user);
        IEnumerable<Listing> GetAllByCategory(Category category);
    }

    public class ListingService : IListingService
    {
        private readonly AppSettings _appSettings;

        public ListingService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public ListingResponse PostListing(ListingRequest model, Category category, User user)
        {
            var listing = new Listing(model, category, user);
            Listing._listings.Add(listing);

            return new ListingResponse(listing);
        }

        public ListingResponse DeleteListing(int listingId, User user)
        {
            var listingQuery = Listing._listings.Where(x => x.Id == listingId);

            // only allow RegisteredUser to delete own listings
            if (user.MaxRole == Roles.RegisteredUser )
            {
                listingQuery = listingQuery.Where(x => x.CreatedBy == user);
            }

            var listing = listingQuery.SingleOrDefault();

            if (listing == null)
                return null;

            listing.IsActive = false;

            return new ListingResponse(listing);
        }

        public ListingResponse PatchListing(int listingId, ListingRequest listingRequest, User user)
        {
            var listing = GetById(listingId);

            if (listing == null)
                return null;

            if (user.MaxRole == Roles.RegisteredUser)
            {
                if (listing.CreatedBy != user)
                    return null;
            }

            listing.Title = listingRequest.Title;
            listing.Content = listingRequest.Content;
            listing.IsPublic = listingRequest.IsPublic;
            listing.IsTrashed = listingRequest.IsTrashed;

            return new ListingResponse(listing);
        }

        public ListingResponse UpdateIsTrashed(int listingId, bool isTrashed)
        {
            var listing = GetById(listingId);

            if (listing == null)
                return null;

            listing.IsTrashed = isTrashed;

            return new ListingResponse(listing);
        }

        public IEnumerable<Listing> GetAll(User user)
        {
            var listings = Listing._listings.Where(x => x.IsActive);

            if ((user?.MaxRole ?? Roles.Visitor) == Roles.Visitor)
            {
                return listings.Where(x => x.IsPublic && !x.IsTrashed);
            }
            if (user.MaxRole == Roles.RegisteredUser)
            {
                return listings.Where(x => !x.IsTrashed);
            }

            return Listing._listings;
        }

        public IEnumerable<Listing> GetAllByUser(User user)
        {
            return Listing._listings.Where(x => x.CreatedBy == user && x.IsActive);
        }

        public Listing GetById(int id)
        {
            return Listing._listings.FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public ListingResponse PostReply(int listingId, ReplyRequest model, User user)
        {
            var listing = GetById(listingId);

            if (listing == null)
                return null;

            var expiration = listing.CreatedOn.AddYears(1);

            if (DateTime.Now > expiration)
                return null;

            var reply = new Reply(model);
            listing.Replies.Add(reply);

            return new ListingResponse(listing);
        }

        public IEnumerable<Reply> GetAllReplies(int listingId, User user)
        {
            var listing = GetById(listingId);

            if (listing == null)
                return null;

            if (user.MaxRole == Roles.RegisteredUser)
            {
                if (listing.CreatedBy != user)
                    return null;
            }

            return listing.Replies;
        }

        public IEnumerable<Listing> GetAllByCategory(Category category)
        {
            var listings = Listing._listings.Where(x => x.Category == category);

            return listings;
        }
    }
}