//using System.ComponentModel.DataAnnotations;
//using System.Text.RegularExpressions;

//namespace Daor_E_Commerce.Application.DTOs.Auth
//{
//    public class RegisterDto
//    {
//        [Required(ErrorMessage = "Name is required")]
//        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must contain only letters")]
//        [StringLength(50, MinimumLength = 3)]
//        public string Name { get; set; } 

//        [Required]
//        [EmailAddress(ErrorMessage = "Invalid Email format")]
//        public string Email { get; set; }

//        [Required]
//        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
//        [RegularExpression(
//            @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).+$",
//            ErrorMessage = "Password must have uppercase, lowercase, number & special character")]
//        public string Password { get; set; }
//    }
//}
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
