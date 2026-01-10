using Daor_E_Commerce.DTOs;

namespace Daor_E_Commerce.Interfaces
{
    public interface ICartService
    {
        Task<string> AddToCartAsync(int userId, AddToCartDto dto);
        Task<object> GetUserCartAsync(int userId);
    }
}