﻿using Daveslist.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daveslist.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public List<Roles> Roles { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        // hardcoding for simplicity, store in a db with hashed passwords in production applications
        [JsonIgnore]
        public static int _nextUserId { get; set; } = 1;
        [JsonIgnore]
        public static List<User> _users { get; set; } = new List<User>();

        public User(RegisterRequest model)
        {
            Id = _nextUserId;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Username = model.Username;
            Email = model.Email;
            Password = model.Password;
            Roles = new List<Roles>();

            _nextUserId++;
        }
    }
}