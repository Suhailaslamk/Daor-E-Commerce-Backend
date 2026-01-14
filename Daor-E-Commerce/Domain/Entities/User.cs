//namespace Daor_E_Commerce.Domain.Entities
//{
//    public class User
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string PasswordHash { get; set; }
//        public string Role { get; set; }
//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    }
//}



using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public bool IsEmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
