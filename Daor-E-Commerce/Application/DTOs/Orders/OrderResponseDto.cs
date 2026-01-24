using Daor_E_Commerce.Domain.Enums;



namespace Daor_E_Commerce.Application.DTOs.Orders
{
    public class OrderItemResponseDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
