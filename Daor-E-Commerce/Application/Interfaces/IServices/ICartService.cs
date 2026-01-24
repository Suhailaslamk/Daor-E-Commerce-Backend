using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Cart;

namespace Daor_E_Commerce.Application.Interfaces.IServices
{
    public interface ICartService
    {
        Task<ApiResponse<string>> AddToCart(int userId, AddToCartDto dto);
        Task<ApiResponse<string>> UpdateCart(int userId, UpdateCartItemDto dto);
        Task<ApiResponse<string>> RemoveFromCart(int userId, int productId);
        Task<ApiResponse<object>> GetCart(int userId);
    }
}


