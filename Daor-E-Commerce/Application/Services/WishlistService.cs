using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Infrastructure.Data;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;

        public WishlistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> ToggleWishlistAsync(int userId, int productId)
        {
            var existing = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (existing != null)
            {
                _context.WishlistItems.Remove(existing);
                await _context.SaveChangesAsync();
                return "Removed from Wishlist";
            }

            var newItem = new WishlistItem
            {
                UserId = userId,
                ProductId = productId
            };

            _context.WishlistItems.Add(newItem);
            await _context.SaveChangesAsync();
            return "Added to Wishlist";
        }

        public async Task<IEnumerable<object>> GetUserWishlistAsync(int userId)
        {
            return await _context.WishlistItems
                .Where(w => w.UserId == userId)
                .Select(w => new
                {
                    w.ProductId,
                    ProductName = w.Product.Name,
                    w.Product.Price,
                    w.Product.ImageUrl
                })
                .ToListAsync();
        }
        public async Task ClearWishlist(int userId)
        {
            var items = await _context.WishlistItems
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (!items.Any())
                return;

            _context.WishlistItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

    }
}