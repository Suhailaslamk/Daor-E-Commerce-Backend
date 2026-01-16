


using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Domain.Entities
{
    public class OtpCode
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public User User { get; set; } = null!;
    }
}
