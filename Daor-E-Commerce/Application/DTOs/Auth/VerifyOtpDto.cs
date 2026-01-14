using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Auth
{
    public class VerifyOtpDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Otp { get; set; } = null!;
    }
}
