using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        public string Password { get; set; } 
    }
}
