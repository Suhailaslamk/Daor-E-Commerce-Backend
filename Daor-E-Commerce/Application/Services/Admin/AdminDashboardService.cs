using Daor_E_Commerce.Application.DTOs.Admin.Dashboard;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Enums;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services.Admin;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly AppDbContext _context;

    public AdminDashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<AdminDashboardDto>> GetDashboard()
    {
        var dashboard = new AdminDashboardDto
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalOrders = await _context.Orders.CountAsync(),
            TotalRevenue = await _context.Orders
                .Where(x => x.Status == OrderStatus.Delivered)
                .SumAsync(x => x.TotalAmount),

            PendingOrders = await _context.Orders
                .CountAsync(x => x.Status == OrderStatus.Pending),

            DeliveredOrders = await _context.Orders
                .CountAsync(x => x.Status == OrderStatus.Delivered)
        };

        return new ApiResponse<AdminDashboardDto>(200, "Dashboard data fetched", dashboard);
    }
}
