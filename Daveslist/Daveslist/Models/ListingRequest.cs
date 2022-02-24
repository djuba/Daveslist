using System.ComponentModel.DataAnnotations;

namespace Daveslist.Models
{
    public class ListingRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Content { get; set; }

        //[Required]
        //public int CategorId { get; set; }

        public bool IsPublic { get; set; }

        public bool IsTrashed { get; set; }
    }
}