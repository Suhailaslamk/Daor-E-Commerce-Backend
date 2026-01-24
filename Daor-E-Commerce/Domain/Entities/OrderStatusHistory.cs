using Daor_E_Commerce.Domain.Enums;

namespace Daor_E_Commerce.Domain.Entities
{
    public class OrderStatusHistory : BaseEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public OrderStatus Status { get; set; }

        public string? Note { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
