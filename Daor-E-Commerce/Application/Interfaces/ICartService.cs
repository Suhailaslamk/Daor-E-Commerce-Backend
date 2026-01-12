using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Cart;

namespace Daor_E_Commerce.Application.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<object>> GetMyCart(int userId);
        Task<ApiResponse<string>> AddToCart(int userId, AddToCartDto dto);
        Task<ApiResponse<string>> UpdateCartItem(int userId, UpdateCartItemDto dto);
        Task<ApiResponse<string>> RemoveCartItem(int userId, int productId);
        Task<ApiResponse<string>> ClearCart(int userId);
    }
}