namespace Daor_E_Commerce.Application.DTOs.Admin.Users
{
    public class AdminUserListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
