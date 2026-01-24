using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }= null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        [Required]
        [MaxLength(200)]
        public string NormalizedName { get; set; } = null!;
        public string ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsActive { get; set; } = true;


        
        }

    }

