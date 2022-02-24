using System.ComponentModel.DataAnnotations;

namespace Daveslist.Models
{
    public class ReplyRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Content { get; set; }
    }
}