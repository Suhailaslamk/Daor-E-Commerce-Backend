using Daor_E_Commerce.Application.DTOs.Cart;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<Cart> GetOrCreateCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<ApiResponse<object>> GetMyCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return new ApiResponse<object>(200, "Cart is empty", new List<object>());

            var result = cart.CartItems.Select(ci => new
            {
                ci.ProductId,
                ci.Quantity
            });

            return new ApiResponse<object>(200, "Cart fetched", result);
        }

        public async Task<ApiResponse<string>> AddToCart(int userId, AddToCartDto dto)
        {
            var cart = await GetOrCreateCart(userId);

            var item = cart.CartItems
                .FirstOrDefault(i => i.ProductId == dto.ProductId);

            if (item != null)
                item.Quantity += dto.Quantity;
            else
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });

            await _context.SaveChangesAsync();
            return new ApiResponse<string>(200, "Item added to cart");
        }

        public async Task<ApiResponse<string>> UpdateCartItem(int userId, UpdateCartItemDto dto)
        {
            var cart = await GetOrCreateCart(userId);

            var item = cart.CartItems
                .FirstOrDefault(i => i.ProductId == dto.ProductId);

            if (item == null)
                return new ApiResponse<string>(404, "Item not found in cart");

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Cart updated");
        }

        public async Task<ApiResponse<string>> RemoveCartItem(int userId, int productId)
        {
            var cart = await GetOrCreateCart(userId);

            var item = cart.CartItems
                .FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                return new ApiResponse<string>(404, "Item not found");

            cart.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Item removed");
        }

        public async Task<ApiResponse<string>> ClearCart(int userId)
        {
            var cart = await GetOrCreateCart(userId);

            cart.CartItems.Clear();
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Cart cleared");
        }
    }
}
