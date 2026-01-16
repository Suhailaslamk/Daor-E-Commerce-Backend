
    using Daor_E_Commerce.Application.DTOs.Admin.Dashboard;
    using Daor_E_Commerce.Application.Interfaces.Admin;
    using Daor_E_Commerce.Common;
    using Daor_E_Commerce.Infrastructure.Data;
    using global::Daor_E_Commerce.Application.DTOs.Admin.Dashboard;
    using global::Daor_E_Commerce.Application.Interfaces.Admin;
    using global::Daor_E_Commerce.Common;
    using global::Daor_E_Commerce.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    namespace Daor_E_Commerce.Application.Services.Admin
    {
        public class AdminDashboardService : IAdminDashboardService
        {
            private readonly AppDbContext _context;

            public AdminDashboardService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResponse<AdminDashboardDto>> GetStats()
            {
                var today = DateTime.UtcNow.Date;

                var dto = new AdminDashboardDto
                {
                    TotalUsers = await _context.Users.CountAsync(),
                    TotalProducts = await _context.Products.CountAsync(),
                    ActiveProducts = await _context.Products.CountAsync(p => p.IsActive),

                    TotalOrders = await _context.Orders.CountAsync(),
                    TotalRevenue = await _context.Orders
                        .Where(o => o.Status == "Delivered")
                        .SumAsync(o => o.TotalAmount),

                    TodayOrders = await _context.Orders
                        .CountAsync(o => o.CreatedAt.Date == today)
                };

                return new ApiResponse<AdminDashboardDto>(200, "Dashboard stats", dto);
            }
        }
    }


