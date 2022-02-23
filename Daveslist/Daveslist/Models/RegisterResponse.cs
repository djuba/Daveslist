using Daveslist.Entities;
using System;

namespace Daveslist.Models
{
    public class RegisterResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Roles { get; set; }


        public RegisterResponse(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Roles = user.Roles.Count > 0 ? string.Join(", ", user.Roles) : "";
        }
    }
}