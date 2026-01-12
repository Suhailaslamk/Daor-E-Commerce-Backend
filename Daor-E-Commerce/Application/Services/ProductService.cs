using Daor_E_Commerce.Application.DTOs.Products;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetAll()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    InStock = p.Stock > 0,
                    Category = p.Category.Name
                }).ToListAsync();

            return new ApiResponse<object>(200, "Products fetched", products);
        }

        public async Task<ApiResponse<object>> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
                return new ApiResponse<object>(404, "Product not found");

            return new ApiResponse<object>(200, "Product fetched", product);
        }

        public async Task<ApiResponse<object>> GetByCategory(int categoryId)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();

            return new ApiResponse<object>(200, "Products fetched", products);
        }

        public async Task<ApiResponse<object>> GetPaged(int page, int pageSize)
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<object>(200, "Paged products", products);
        }

        public async Task<ApiResponse<object>> Search(string search, int page, int pageSize)
        {
            var products = await _context.Products
                .Where(p => p.IsActive && p.Name.Contains(search))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ApiResponse<object>(200, "Search results", products);
        }

        public async Task<ApiResponse<object>> FilterAndSort(ProductFilterDto dto)
        {
            var query = _context.Products.AsQueryable();

            if (dto.ProductId.HasValue)
                query = query.Where(p => p.Id == dto.ProductId);

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(p => p.Name.Contains(dto.Name));

            if (dto.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == dto.CategoryId);

            if (dto.MinPrice.HasValue)
                query = query.Where(p => p.Price >= dto.MinPrice);

            if (dto.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= dto.MaxPrice);

            if (dto.InStock.HasValue)
                query = query.Where(p => (p.Stock > 0) == dto.InStock);

            if (dto.IsActive.HasValue)
                query = query.Where(p => p.IsActive == dto.IsActive);

            query = dto.SortBy?.ToLower() switch
            {
                "name" => dto.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "createdon" => dto.Descending ? query.OrderByDescending(p => p.CreatedOn) : query.OrderBy(p => p.CreatedOn),
                _ => dto.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price)
            };

            var result = await query
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            return new ApiResponse<object>(200, "Filtered products", result);
        }
    }
}
