namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<string> ToggleWishlistAsync(int userId, int productId);
        Task<IEnumerable<object>> GetUserWishlistAsync(int userId);
        Task ClearWishlist(int userId);
    }
}