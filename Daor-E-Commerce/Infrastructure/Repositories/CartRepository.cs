using Daor_E_Commerce.Application.Interfaces.Repositories;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cart?> GetUserCartAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    !c.IsDeleted);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cartId &&
                    ci.ProductId == productId &&
                    !ci.IsDeleted);
        }

        public async Task<List<CartItem>> GetSelectedCartItemsAsync(
            int userId,
            List<int> cartItemIds)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci =>
                    cartItemIds.Contains(ci.Id) &&
                    ci.Cart.UserId == userId &&
                    !ci.IsDeleted)
                .ToListAsync();
        }
    }
}
