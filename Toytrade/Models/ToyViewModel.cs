using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ViewModels
{
    public class ToyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        public string Condition { get; set; }

        [Display(Name = "Image URL")]
        public string? Image { get; set; } // Image is now optional

        public string? Username { get; set; } // No validation required (set in service)
    }
}
