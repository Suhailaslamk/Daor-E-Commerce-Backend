using Daor_E_Commerce.Common;
namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<ApiResponse<string>> ToggleWishlist(int userId, int productId);
        Task<ApiResponse<object>> GetWishlist(int userId);
        Task<ApiResponse<string>> ClearWishlist(int userId);
    }
}