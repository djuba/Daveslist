using System.ComponentModel.DataAnnotations;

namespace Daveslist.Models
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; }

        public bool IsPublic { get; set; }
    }
}