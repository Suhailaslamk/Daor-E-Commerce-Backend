using Daor_E_Commerce.Application.DTOs.Admin.Category;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services.Admin
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly AppDbContext _context;

        public AdminCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<int>> Create(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return new ApiResponse<int>(400, "Category name is required");

            var exists = await _context.Categories
                .AnyAsync(c => c.Name == dto.Name && !c.IsDeleted);

            if (exists)
                return new ApiResponse<int>(400, "Category already exists");

            var category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new ApiResponse<int>(201, "Category created", category.Id);
        }

        public async Task<ApiResponse<string>> Update(UpdateCategoryDto dto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && !c.IsDeleted);

            if (category == null)
                return new ApiResponse<string>(404, "Category not found");

            category.Name = dto.Name;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Category updated");
        }

        public async Task<ApiResponse<string>> Delete(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);

            if (category == null)
                return new ApiResponse<string>(404, "Category not found");

            category.IsDeleted = true;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Category deleted");
        }

        public async Task<ApiResponse<object>> GetAll()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .ToListAsync();

            return new ApiResponse<object>(200, "Categories fetched", categories);
        }
    }
}
