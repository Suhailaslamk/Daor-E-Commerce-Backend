using System.ComponentModel.DataAnnotations;
namespace Daor_E_Commerce.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public List<int> CartItemIds { get; set; } = new();
       
        [Required]

        public int ShippingAddressId { get; set; }

    }
}