using Daveslist.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Daveslist.Entities
{
    public class Reply
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Content { get; set; }

        public Reply (ReplyRequest model)
        {
            Content = model.Content;
        }
    }
}
