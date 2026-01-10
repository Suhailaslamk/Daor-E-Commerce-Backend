using Daor_E_Commerce.Data;
using Daor_E_Commerce.Interfaces;
using Daor_E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> PlaceOrderAsync(int userId)
        {
            // 1. Get the cart with its items and product details
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return "Cart is empty";

            // 2. Create the Order object
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Save to generate Order.Id

            // 3. Convert CartItems to OrderItems
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price // Snapshot the price at time of purchase
                };
                _context.OrderItems.Add(orderItem);
            }

            // 4. Clear the User's Cart
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return $"Order #{order.Id} placed successfully!";
        }

        public async Task<IEnumerable<object>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    Items = o.OrderItems.Select(oi => new
                    {
                        oi.Product.Name,
                        oi.Quantity,
                        oi.Price
                    })
                })
                .ToListAsync();
        }
    }
}