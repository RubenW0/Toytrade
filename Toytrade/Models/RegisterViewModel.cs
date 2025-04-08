using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Address { get; set; }
    }
}
