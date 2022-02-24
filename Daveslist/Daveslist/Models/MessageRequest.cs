using System.ComponentModel.DataAnnotations;

namespace Daveslist.Models
{
    public class MessageRequest
    {
        [Required]
        public string Content { get; set; }
    }
}