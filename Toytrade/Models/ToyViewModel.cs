using PresentationLayer.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class ToyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        public ToyCondition Condition { get; set; }

        public string? Image { get; set; }

        public string? Username { get; set; }

        public IFormFile? ImageFile { get; set; }
        public int UserId { get; set; }
    }
}
