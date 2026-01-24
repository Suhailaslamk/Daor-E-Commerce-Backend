using Daor_E_Commerce.Application.Interfaces.IRepositories;
using Daor_E_Commerce.Domain.Entities;

namespace Daor_E_Commerce.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetActiveByIdAsync(int id);
        Task<List<Product>> GetActiveProductsAsync();
        Task<bool> HasStockAsync(int productId, int quantity);
    }
}