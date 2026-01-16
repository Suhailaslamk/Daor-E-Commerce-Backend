using System.ComponentModel.DataAnnotations;
namespace Daor_E_Commerce.Application.DTOs.Admin.Product
{
    public class AddProductDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Range(1, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int Stock { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}