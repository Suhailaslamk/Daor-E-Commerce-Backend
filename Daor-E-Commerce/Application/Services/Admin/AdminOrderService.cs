using Daor_E_Commerce.Application.DTOs.Admin.Order;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Enums;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services.Admin;

public class AdminOrderService : IAdminOrderService
{
    private readonly AppDbContext _context;

    public AdminOrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<object>> GetAll()
    {
        var orders = await _context.Orders
            .Include(x => x.User)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.OrderDate)
            .Select(x => new
            {
                x.Id,
                x.User.Email,
                x.Status,
                x.TotalAmount,
                x.OrderDate
            })
            .ToListAsync();

        return new ApiResponse<object>(200, "Orders fetched", orders);
    }

    public async Task<ApiResponse<object>> GetById(int orderId)
    {
        var order = await _context.Orders
            .Include(x => x.User)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
            return new ApiResponse<object>(404, "Order not found");

        return new ApiResponse<object>(200, "Order fetched", new
        {
            order.Id,
            order.User.Email,
            order.Status,
            order.TotalAmount,
            Items = order.OrderItems.Select(i => new
            {
                i.Product.Name,
                i.Quantity,
                i.Price
            })
        });
    }

    public async Task<ApiResponse<string>> UpdateStatus(UpdateOrderStatusDto dto)
    {
        var order = await _context.Orders.FindAsync(dto.OrderId);

        if (order == null)
            return new ApiResponse<string>(404, "Order not found");

        if (!IsValidStatusFlow(order.Status, dto.Status))
            return new ApiResponse<string>(400, "Invalid order status transition");

        order.Status = dto.Status;
        await _context.SaveChangesAsync();

        return new ApiResponse<string>(200, "Order status updated");
    }

    private bool IsValidStatusFlow(OrderStatus current, OrderStatus next)
    {
        return next switch
        {
            OrderStatus.Paid => current == OrderStatus.Pending,
            OrderStatus.Processing => current == OrderStatus.Paid,
            OrderStatus.Shipped => current == OrderStatus.Processing,
            OrderStatus.Delivered => current == OrderStatus.Shipped,
            OrderStatus.Cancelled => current is OrderStatus.Pending or OrderStatus.Paid,
            _ => false
        };
    }
}
