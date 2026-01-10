using Daor_E_Commerce.Models;

namespace Daor_E_Commerce.Interfaces
{
    public interface IProductService
    {
        // Get all products, or filter them if a categoryId is provided
        Task<IEnumerable<Product>> GetProductsAsync(int? categoryId);

        // Get details of one specific product
        Task<Product> GetProductByIdAsync(int id);

        // Get the list of all categories (for your React sidebar)
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}