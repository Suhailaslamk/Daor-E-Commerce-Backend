



using Daor_E_Commerce.Application.DTOs.Orders;
using Daor_E_Commerce.Application.Interfaces.IServices;
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

    

    public async Task<ApiResponse<object>> CreateOrder(int userId, CreateOrderDto dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        var address = await _context.ShippingAddresses
            .FirstOrDefaultAsync(a => a.Id == dto.ShippingAddressId && a.UserId == userId);

        if (address == null)
            return new ApiResponse<object>(400, "Invalid shipping address");

        var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci =>
                ci.Cart.UserId == userId &&
                dto.CartItemIds.Contains(ci.Id))
            .ToListAsync();

        if (!cartItems.Any())
            return new ApiResponse<object>(400, "No valid cart items selected");

        var order = new Order
        {
            UserId = userId,
            Status = OrderStatus.PaymentPending,
            ShippingAddressId = address.Id,
            OrderItems = new List<OrderItem>()
        };

        decimal total = 0;

        foreach (var item in cartItems)
        {
            if (item.Product.Stock <= 0)
                return new ApiResponse<object>(400, $"Product {item.Product.Name} is out of stock");

            if (item.Product.Stock < item.Quantity)
                return new ApiResponse<object>(400, $"Insufficient stock for {item.Product.Name}");

            total += item.Product.Price * item.Quantity;
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.Price
            });
        }

        order.TotalAmount = total;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        _context.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = OrderStatus.PaymentPending,
            Note = "Order created, awaiting payment"
        });

        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return new ApiResponse<object>(201, "Order created", new
        {
            order.Id,
            order.TotalAmount
        });
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
            Status=order.Status.ToString(),
            order.OrderDate,
            Items = order.OrderItems.Select(i => new
            {
                ProductName =i.Product.Name,
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
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o =>
                o.Id == orderId &&
                o.UserId == userId);

        if (order == null)
            return new ApiResponse<string>(404, "Order not found");

        if (order.Status == OrderStatus.Cancelled)
            return new ApiResponse<string>(400, "Order already cancelled");

        if (order.Status == OrderStatus.Paid)
            return new ApiResponse<string>(400, "Paid orders cannot be cancelled");

        foreach (var item in order.OrderItems)
        {
            item.Product.Stock += item.Quantity;
        }

        order.Status = OrderStatus.Cancelled;
        _context.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = OrderStatus.Cancelled,
            Note = "Order cancelled by user"
        });
        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Order cancelled successfully");
    }


    //public async Task<ApiResponse<string>> VerifyPayment(int userId, VerifyPaymentDto dto)
    //{
    //    using var tx = await _context.Database.BeginTransactionAsync();

    //    var order = await _context.Orders
    //        .Include(o => o.OrderItems)
    //        .ThenInclude(oi => oi.Product)
    //        .FirstOrDefaultAsync(o =>
    //            o.Id == dto.OrderId &&
    //            o.UserId == userId &&
    //            o.Status == OrderStatus.PaymentPending);

    //    if (order == null)
    //        return new ApiResponse<string>(400, "Invalid order");

    //    foreach (var item in order.OrderItems)
    //    {
    //        if (item.Product.Stock < item.Quantity)
    //            return new ApiResponse<string>(400, "Stock issue during payment");

    //        item.Product.Stock -= item.Quantity;
    //    }


    //    var cartItems = _context.CartItems
    //       .Where(ci => ci.Cart.UserId == userId);

    //    _context.CartItems.RemoveRange(cartItems);

    //    order.Status = OrderStatus.Paid;
    //    order.PaymentIntentId = dto.PaymentIntentId;

    //    // Add status history
    //    _context.OrderStatusHistories.Add(new OrderStatusHistory
    //    {
    //        OrderId = order.Id,
    //        Status = OrderStatus.Paid,
    //        Note = "Payment verified and order placed"
    //    });

    //    // Remove cart items
    //    var cartItems = await _context.CartItems
    //        .Where(ci => ci.Cart.UserId == userId)
    //        .ToListAsync();

    //    _context.CartItems.RemoveRange(cartItems);

    //    await _context.SaveChangesAsync();






    //    order.Status = OrderStatus.Paid;
    //    order.PaymentIntentId = dto.PaymentIntentId;

    //    _context.OrderStatusHistories.Add(new OrderStatusHistory
    //    {
    //        OrderId = order.Id,
    //        Status = OrderStatus.Paid,
    //        Note = "Payment verified"
    //    });

    //    await _context.SaveChangesAsync();
    //    await tx.CommitAsync();

    //    return new ApiResponse<string>(200, "Payment successful");
    //}

    //public async Task<ApiResponse<string>> VerifyPayment(int userId, VerifyPaymentDto dto)
    //{
    //    using var tx = await _context.Database.BeginTransactionAsync();

    //    var order = await _context.Orders
    //        .Include(o => o.OrderItems)
    //        .ThenInclude(oi => oi.Product)
    //        .FirstOrDefaultAsync(o =>
    //            o.Id == dto.OrderId &&
    //            o.UserId == userId &&
    //            o.Status == OrderStatus.PaymentPending);

    //    if (order == null)
    //        return new ApiResponse<string>(400, "Invalid order");

    //    var existingPayment = await _context.Orders
    //        .AnyAsync(o => o.PaymentIntentId == dto.PaymentIntentId && o.Status == OrderStatus.Paid);

    //    if (existingPayment)
    //        return new ApiResponse<string>(400, "Payment already processed for this order");

    //    foreach (var item in order.OrderItems)
    //    {
    //        if (item.Product.Stock < item.Quantity)
    //            return new ApiResponse<string>(400, $"Insufficient stock for {item.Product.Name}");

    //        item.Product.Stock -= item.Quantity;
    //    }

    //    var cartItems = await _context.CartItems
    //        .Where(ci => ci.Cart.UserId == userId)
    //        .ToListAsync();

    //    _context.CartItems.RemoveRange(cartItems);

    //    var wishlistItems = await _context.WishlistItems
    //        .Where(w => w.UserId == userId && order.OrderItems.Select(oi => oi.ProductId).Contains(w.ProductId))
    //        .ToListAsync();

    //    _context.WishlistItems.RemoveRange(wishlistItems);

    //    order.Status = OrderStatus.Paid;
    //    order.PaymentIntentId = dto.PaymentIntentId;

    //    _context.OrderStatusHistories.Add(new OrderStatusHistory
    //    {
    //        OrderId = order.Id,
    //        Status = OrderStatus.Paid,
    //        Note = "Payment verified and order placed"
    //    });

    //    await _context.SaveChangesAsync();
    //    await tx.CommitAsync();

    //    var responseDto = new OrderPaymentResponseDto
    //    {
    //        OrderId = order.Id,
    //        TotalAmount = order.TotalAmount,
    //        Status = order.Status,
    //        Items = order.OrderItems.Select(i => new OrderItemResponseDto
    //        {
    //            ProductName = i.Product.Name,
    //            Quantity = i.Quantity,
    //            Price = i.Price
    //        }).ToList()
    //    };

    //    return new ApiResponse<OrderPaymentResponseDto>(200, "Payment successful and order placed", responseDto);
    //}
    public async Task<ApiResponse<OrderPaymentResponseDto>> VerifyPayment(
    int userId,
    VerifyPaymentDto dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o =>
                o.Id == dto.OrderId &&
                o.UserId == userId &&
                o.Status == OrderStatus.PaymentPending);

        if (order == null)
            return new ApiResponse<OrderPaymentResponseDto>(400, "Invalid order");

        var existingPayment = await _context.Orders
            .AnyAsync(o => o.PaymentIntentId == dto.PaymentIntentId && o.Status == OrderStatus.Paid);

        if (existingPayment)
            return new ApiResponse<OrderPaymentResponseDto>(400, "Payment already processed");



        if (string.IsNullOrWhiteSpace(dto.PaymentIntentId))
            return new ApiResponse<OrderPaymentResponseDto>(400, "Invalid payment reference");

        bool alreadyUsed = await _context.Orders
            .AnyAsync(o =>
                o.PaymentIntentId == dto.PaymentIntentId &&
                o.Id != order.Id);

        if (alreadyUsed)
            return new ApiResponse<OrderPaymentResponseDto>(400, "Payment reference already used");


        foreach (var item in order.OrderItems)
        {
            if (item.Product.Stock < item.Quantity)
            {
                await tx.RollbackAsync();
                return new ApiResponse<OrderPaymentResponseDto>(
                    400,
                    $"Insufficient stock for {item.Product.Name}");
            }

            item.Product.Stock -= item.Quantity;
        }

        var cartItems = await _context.CartItems
            .Where(ci => ci.Cart.UserId == userId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);

        var wishlistItems = await _context.WishlistItems
            .Where(w =>
                w.UserId == userId &&
                order.OrderItems.Select(oi => oi.ProductId).Contains(w.ProductId))
            .ToListAsync();

        _context.WishlistItems.RemoveRange(wishlistItems);

        order.Status = OrderStatus.Paid;
        order.PaymentIntentId = dto.PaymentIntentId;

        _context.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = OrderStatus.Paid,
            Note = "Payment verified and order placed"
        });

        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        var responseDto = new OrderPaymentResponseDto
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(i => new OrderItemResponseDto
            {
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        return new ApiResponse<OrderPaymentResponseDto>(
            200,
            "Payment successful and order placed",
            responseDto);
    }


}