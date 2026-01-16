namespace Daor_E_Commerce.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public ICollection<Product> Products { get; set; } = new List<Product>();

    
    }
}
