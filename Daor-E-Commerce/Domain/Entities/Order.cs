using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Domain.Enums;

namespace Daor_E_Commerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? PaymentIntentId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public int ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        
    }
}

