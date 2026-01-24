using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Cart
{
    public class UpdateCartItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
