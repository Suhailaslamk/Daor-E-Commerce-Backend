



using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Daor_E_Commerce.Common;
using Microsoft.EntityFrameworkCore;


public class WishlistService : IWishlistService
{
    private readonly AppDbContext _context;

    public WishlistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<string>> ToggleWishlist(int userId, int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null || !product.IsActive)
            return new ApiResponse<string>(404, "Product not found");

        var item = await _context.WishlistItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (item != null)
        {
            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return new ApiResponse<string>(200, "Removed from wishlist");
        }

        _context.WishlistItems.Add(new WishlistItem
        {
            UserId = userId,
            ProductId = productId
        });

        await _context.SaveChangesAsync();
        return new ApiResponse<string>(200, "Added to wishlist");
    }

    public async Task<ApiResponse<object>> GetWishlist(int userId)
    {
        var items = await _context.WishlistItems
            .Include(x => x.Product)
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                x.Product.Id,
                x.Product.Name,
                x.Product.Price,
                x.Product.ImageUrl
            })
            .ToListAsync();

        return new ApiResponse<object>(200, "Wishlist retrieved", items);
    }

    public async Task<ApiResponse<string>> ClearWishlist(int userId)
    {
        var items = _context.WishlistItems.Where(x => x.UserId == userId);
        _context.WishlistItems.RemoveRange(items);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Wishlist cleared");
    }
}
