using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Admin.Users
{
    public class AdminUserListDto
    {
        public int Id { get; set; }

        [Required] 
        public string FullName { get; set; } = "";
        [Required]
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
