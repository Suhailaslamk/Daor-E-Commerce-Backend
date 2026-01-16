using Daor_E_Commerce.Application.DTOs.Admin.Order;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services.Admin
{
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
                .Include(o => o.User)
                .Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.User.Email,
                    o.TotalAmount,
                    o.Status,
                    o.OrderDate
                })
                .ToListAsync();

            return new ApiResponse<object>(200, "Orders fetched", orders);
        }

        public async Task<ApiResponse<object>> GetById(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return new ApiResponse<object>(404, "Order not found");

            var result = new
            {
                order.Id,
                order.User.Email,
                order.Status,
                order.TotalAmount,
                order.OrderDate,
                Items = order.OrderItems.Select(i => new
                {
                    i.ProductId,
                    i.Product.Name,
                    i.Quantity,
                    i.Price
                })
            };

            return new ApiResponse<object>(200, "Order details fetched", result);
        }

        public async Task<ApiResponse<string>> UpdateStatus(UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null)
                return new ApiResponse<string>(404, "Order not found");

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Order status updated");
        }
    }
}
