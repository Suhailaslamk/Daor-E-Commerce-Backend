
namespace Daor_E_Commerce.Application.DTOs.Products
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
