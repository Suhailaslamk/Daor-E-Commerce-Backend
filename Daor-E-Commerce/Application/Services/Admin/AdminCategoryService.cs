using Daor_E_Commerce.Application.DTOs.Admin.Category;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Daor_E_Commerce.Application.Services.Admin
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly AppDbContext _context;

        public AdminCategoryService(AppDbContext context)
        {
            _context = context;
        }
        private static string NormalizeCategoryName(string name)
        {
            return Regex
                .Replace(name.Trim().ToLower(), @"\s+", "");
        }
        public async Task<ApiResponse<int>> Create(CreateCategoryDto dto)
        {

            dto.Name = dto.Name?.Trim();

            if (string.IsNullOrWhiteSpace(dto.Name))
                return new ApiResponse<int>(400, "Category name is required");

            if (dto.Name.Length < 2 || dto.Name.Length > 50)
                return new ApiResponse<int>(400, "Category name must be 2–50 characters");

            if (!Regex.IsMatch(dto.Name, @"^[A-Za-z][A-Za-z &-]*$"))
                return new ApiResponse<int>(400, "Invalid category name");



            var normalizedName = NormalizeCategoryName(dto.Name);

            bool exists = await _context.Categories.AnyAsync(c =>
                c.NormalizedName == normalizedName && !c.IsDeleted);

            if (exists)
                return new ApiResponse<int>(400, "Category already exists");

            var category = new Category
            {
                Name = dto.Name,
                NormalizedName = normalizedName   
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new ApiResponse<int>(201, "Category created", category.Id);
        }

        public async Task<ApiResponse<string>> Update(UpdateCategoryDto dto)
        {


            dto.Name = dto.Name?.Trim();
            var normalizedName = NormalizeCategoryName(dto.Name);

            bool exists = await _context.Categories.AnyAsync(c =>
                c.Id != dto.CategoryId &&
                c.NormalizedName == normalizedName &&
                !c.IsDeleted);

            

           


            var category = await _context.Categories

                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && !c.IsDeleted);
            if (exists)
                return new ApiResponse<string>(400, "Category already exists");
            if (category == null)
                return new ApiResponse<string>(404, "Category not found");

            category.Name = dto.Name;
            category.NormalizedName = normalizedName;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Category updated");
        }

        public async Task<ApiResponse<string>> Delete(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);

            if (category == null)
                return new ApiResponse<string>(404, "Category not found");

            bool hasProducts = await _context.Products
            .AnyAsync(p => p.CategoryId == categoryId && !p.IsDeleted);

            if (hasProducts)
                return new ApiResponse<string>(400, "Cannot delete category with products");
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
