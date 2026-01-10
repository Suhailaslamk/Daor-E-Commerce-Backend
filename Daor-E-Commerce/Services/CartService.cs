using Daor_E_Commerce.Data;
using Daor_E_Commerce.DTOs;
using Daor_E_Commerce.Interfaces;
using Daor_E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        public CartService(AppDbContext context) { _context = context; }

        public async Task<string> AddToCartAsync(int userId, AddToCartDto dto)
        {
            // 1. Find or Create a Cart for this User
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // 2. Check if product is already in the cart
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == dto.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += dto.Quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return "Added to cart successfully";
        }

        public async Task<object> GetUserCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    CartId = c.Id,
                    Items = c.CartItems.Select(ci => new
                    {
                        ci.ProductId,
                        ProductName = ci.Product.Name,
                        Price = ci.Product.Price,
                        ci.Quantity,
                        Total = ci.Quantity * ci.Product.Price
                    }).ToList()
                }).FirstOrDefaultAsync();

            // If cart is null, return a clean "Empty" object instead of nothing
            if (cart == null)
            {
                return new { Message = "Cart is empty", Items = new List<object>() };
            }

            return cart;
        }
    }
    }
