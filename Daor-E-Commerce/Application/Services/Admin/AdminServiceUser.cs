using Daor_E_Commerce.Application.DTOs.Admin.Users;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly AppDbContext _context;

        public AdminUserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetAll(int page, int pageSize)
        {
            var query = _context.Users.AsQueryable();

            var users = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AdminUserListDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    IsBlocked = x.IsBlocked,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            var total = await query.CountAsync();

            return new ApiResponse<object>(200, "Users list", new
            {
                total,
                page,
                pageSize,
                users
            });
        }

        public async Task<ApiResponse<object>> GetById(int userId)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.IsBlocked,
                    x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return new ApiResponse<object>(404, "User not found");

            return new ApiResponse<object>(200, "User details", user);
        }

        public async Task<ApiResponse<string>> BlockUser(int userId, bool block)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            user.IsBlocked = block;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(
                200,
                block ? "User blocked" : "User unblocked"
            );
        }

        public async Task<ApiResponse<object>> GetUserOrders(int userId)
        {
            var orders = await _context.Orders
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new
                {
                    x.Id,
                    x.TotalAmount,
                    x.Status,
                    x.OrderDate
                })
                .ToListAsync();

            return new ApiResponse<object>(200, "User orders", orders);
        }
    }
}
