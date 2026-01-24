using Daor_E_Commerce.Application.Interfaces.IRepositories;
using Daor_E_Commerce.Domain.Entities;

namespace Daor_E_Commerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<List<Order>> GetUserOrdersAsync(int userId);
    }
}
