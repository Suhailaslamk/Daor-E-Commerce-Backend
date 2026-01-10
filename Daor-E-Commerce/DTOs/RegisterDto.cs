using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
