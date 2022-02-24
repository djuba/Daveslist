using Daveslist.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Daveslist.Entities
{
    public class Message
    {
        [Required]
        public string Content { get; set; }

        public Message(MessageRequest model)
        {
            Content = model.Content;
        }
    }
}
