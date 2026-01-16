using System.ComponentModel.DataAnnotations;
namespace Daor_E_Commerce.Application.DTOs.Admin.Product
{
    public class UpdateProductDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, Range(1, 1000000)]
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;

        [Range(0, 100000)]
        public int Stock { get; set; }

        public bool IsActive { get; set; }
    }
}





