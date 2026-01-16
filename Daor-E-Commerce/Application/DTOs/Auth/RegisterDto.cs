
using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).+$",
            ErrorMessage = "Password must contain uppercase, lowercase, number & special character"
        )]
        public string Password { get; set; } = null!;
    }
}
