using Daor_E_Commerce.Models;

namespace Daor_E_Commerce.Interfaces
{
    public interface IOrderService
    {
        Task<string> PlaceOrderAsync(int userId);
        Task<IEnumerable<object>> GetUserOrdersAsync(int userId);
    }
}