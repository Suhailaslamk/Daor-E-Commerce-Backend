//using Daor_E_Commerce.Application.DTOs.Orders;
//using Daor_E_Commerce.Application.Interfaces;
//using Daor_E_Commerce.Common;
//using Daor_E_Commerce.Domain.Entities;
//using Daor_E_Commerce.Domain.Enums;
//using Daor_E_Commerce.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Daor_E_Commerce.Application.Services
//{
//    public class OrderService : IOrderService
//    {
//        private readonly AppDbContext _context;

//        public OrderService(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ApiResponse<object>> CreateOrder(int userId)
//        {
//            var cart = await _context.Carts
//                .Include(c => c.CartItems)
//                .FirstOrDefaultAsync(c => c.UserId == userId);

//            if (cart == null || !cart.CartItems.Any())
//                return new ApiResponse<object>(400, "Cart is empty");

//            var order = new Order
//            {
//                UserId = userId,
//                TotalAmount = 0
//            };

//            foreach (var item in cart.CartItems)
//            {
//                // TODO: Fetch product price from Product table
//                decimal price = 100; // TEMP placeholder

//                order.OrderItems.Add(new OrderItem
//                {
//                    ProductId = item.ProductId,
//                    Quantity = item.Quantity,
//                    Price = price
//                });

//                order.TotalAmount += price * item.Quantity;
//            }

//            _context.Orders.Add(order);
//            _context.CartItems.RemoveRange(cart.CartItems);

//            await _context.SaveChangesAsync();

//            return new ApiResponse<object>(200, "Order created", new
//            {
//                order.Id,
//                order.TotalAmount,
//                order.Status
//            });
//        }

//        public async Task<ApiResponse<object>> GetMyOrders(int userId)
//        {
//            var orders = await _context.Orders
//                .Where(o => o.UserId == userId)
//                .Select(o => new
//                {
//                    o.Id,
//                    o.TotalAmount,
//                    o.Status,
//                    o.OrderDate
//                })
//                .ToListAsync();

//            return new ApiResponse<object>(200, "Orders fetched", orders);
//        }

//        public async Task<ApiResponse<object>> GetOrderById(int userId, int orderId)
//        {
//            var order = await _context.Orders
//                .Include(o => o.OrderItems)
//                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

//            if (order == null)
//                return new ApiResponse<object>(404, "Order not found");

//            return new ApiResponse<object>(200, "Order fetched", order);
//        }

//        public async Task<ApiResponse<string>> CancelOrder(int userId, int orderId)
//        {
//            var order = await _context.Orders
//                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

//            if (order == null)
//                return new ApiResponse<string>(404, "Order not found");

//            if (order.Status != OrderStatus.Pending)
//                return new ApiResponse<string>(400, "Order cannot be cancelled");

//            order.Status = OrderStatus.Cancelled;
//            await _context.SaveChangesAsync();

//            return new ApiResponse<string>(200, "Order cancelled");
//        }

//        public async Task<ApiResponse<string>> VerifyPayment(VerifyPaymentDto dto)
//        {
//            var order = await _context.Orders.FindAsync(dto.OrderId);

//            if (order == null)
//                return new ApiResponse<string>(404, "Order not found");

//            order.Status = OrderStatus.Paid;
//            await _context.SaveChangesAsync();

//            return new ApiResponse<string>(200, "Payment verified");
//        }
//    }
//}




using Daor_E_Commerce.Application.DTOs.Orders;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Domain.Enums;
using Daor_E_Commerce.Infrastructure.Data;
using System.Linq;
using Daor_E_Commerce.Common;
using Microsoft.EntityFrameworkCore;


public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<string>> CreateOrder(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
            return new ApiResponse<string>(400, "Cart is empty");

        decimal total = 0;

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.Pending,
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in cart.CartItems)
        {
            if (item.Product.Stock < item.Quantity)
                return new ApiResponse<string>(400, $"Insufficient stock for {item.Product.Name}");

            total += item.Product.Price * item.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.Price
            });

            item.Product.Stock -= item.Quantity;
        }

        order.TotalAmount = total;

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cart.CartItems);

        await _context.SaveChangesAsync();

        return new ApiResponse<string>(201, "Order created successfully");
    }

    public async Task<ApiResponse<object>> GetMyOrders(int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new
            {
                o.Id,
                o.TotalAmount,
                o.Status,
                o.OrderDate,
                Items = o.OrderItems.Select(i => new
                {
                    i.Product.Name,
                    i.Quantity,
                    i.Price
                })
            })
            .ToListAsync();

        return new ApiResponse<object>(200, "Orders retrieved", orders);
    }

    public async Task<ApiResponse<object>> GetOrderById(int userId, int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null)
            return new ApiResponse<object>(404, "Order not found");

        var result = new
        {
            order.Id,
            order.TotalAmount,
            order.Status,
            order.OrderDate,
            Items = order.OrderItems.Select(i => new
            {
                i.Product.Name,
                i.Quantity,
                i.Price
            })
        };

        return new ApiResponse<object>(200, "Order details", result);
    }

    public async Task<ApiResponse<string>> CancelOrder(int userId, int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null)
            return new ApiResponse<string>(404, "Order not found");

        if (order.Status != OrderStatus.Pending)
            return new ApiResponse<string>(400, "Order cannot be cancelled");

        order.Status = OrderStatus.Cancelled;

        foreach (var item in order.OrderItems)
        {
            item.Product.Stock += item.Quantity;
        }

        await _context.SaveChangesAsync();
        return new ApiResponse<string>(200, "Order cancelled");
    }

    public async Task<ApiResponse<string>> VerifyPayment(int userId, VerifyPaymentDto dto)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == dto.OrderId && o.UserId == userId);

        if (order == null)
            return new ApiResponse<string>(404, "Order not found");

        order.Status = OrderStatus.Paid;
        order.PaymentIntentId = dto.PaymentReference;

        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Payment verified");
    }
}
