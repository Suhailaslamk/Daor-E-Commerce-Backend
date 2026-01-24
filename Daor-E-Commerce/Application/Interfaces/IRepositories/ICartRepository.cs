using Daor_E_Commerce.Application.Interfaces.IRepositories;
using Daor_E_Commerce.Domain.Entities;

namespace Daor_E_Commerce.Application.Interfaces.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetUserCartAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<List<CartItem>> GetSelectedCartItemsAsync(int userId, List<int> cartItemIds);
    }
}
