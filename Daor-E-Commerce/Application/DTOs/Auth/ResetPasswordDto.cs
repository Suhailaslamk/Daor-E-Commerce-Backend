using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required]
        public string Otp { get; set; } = null!;

        [Required, MinLength(8)]
        public string NewPassword { get; set; } = null!;
    }
}
