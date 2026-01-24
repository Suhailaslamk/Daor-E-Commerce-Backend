namespace Daor_E_Commerce.Application.DTOs.Orders
{
    public class CheckoutDto
    {
        public List<int> CartItemIds { get; set; } = new();

        public int ShippingAddressId { get; set; }
    }
}