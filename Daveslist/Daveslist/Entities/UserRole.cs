using Daveslist.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daveslist.Entities
{
    public class UserRole
    {
        public User User { get; set; }

        public Roles Role { get; set; }
    }
}
