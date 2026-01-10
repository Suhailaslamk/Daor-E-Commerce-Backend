namespace Daor_E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } // Pending, Paid

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
