using Daor_E_Commerce.Domain.Enums;

namespace Daor_E_Commerce.Application.DTOs.Orders
{
    public class OrderPaymentResponseDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new List<OrderItemResponseDto>();
    }
}
