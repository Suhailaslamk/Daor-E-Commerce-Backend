//namespace Daor_E_Commerce.Domain
//{
//    public class RefreshToken
//    {
//        public int Id { get; set; }
//        public int UserId { get; set; }
//        public string Token { get; set; }
//        public DateTime ExpiresAt { get; set; }
//        public bool IsRevoked { get; set; } = false;
//    }
//}


namespace Daor_E_Commerce.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public User User { get; set; } = null!;
    }
}
