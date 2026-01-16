


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

        public async Task<ApiResponse<List<ProductResponseDto>>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    InStock = p.Stock > 0,
                    Category = p.Category!.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return new ApiResponse<List<ProductResponseDto>>(200, "Products fetched", products);
        }

        public async Task<ApiResponse<ProductResponseDto>> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
                return new ApiResponse<ProductResponseDto>(404, "Product not found");

            return new ApiResponse<ProductResponseDto>(200, "Product fetched", new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                InStock = product.Stock > 0,
                Category = product.Category!.Name,
                ImageUrl = product.ImageUrl
            });
        }

        public async Task<ApiResponse<List<ProductResponseDto>>> GetByCategoryAsync(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    InStock = p.Stock > 0,
                    Category = p.Category!.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return new ApiResponse<List<ProductResponseDto>>(200, "Category products fetched", products);
        }
        public async Task<ApiResponse<List<ProductResponseDto>>> FilterAndSortAsync(ProductFilterDto filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            // 🔍 Filters
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => p.Name.Contains(filter.Name));

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice);

            if (filter.InStock.HasValue)
                query = query.Where(p => (p.Stock > 0) == filter.InStock);

            // ↕ Sort
            query = filter.SortBy?.ToLower() switch
            {
                "price" => filter.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name" => filter.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "createdon" => filter.Descending ? query.OrderByDescending(p => p.CreatedOn) : query.OrderBy(p => p.CreatedOn),
                _ => query.OrderBy(p => p.Id)
            };

            // 📄 Pagination
            var products = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    InStock = p.Stock > 0,
                    Category = p.Category!.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return new ApiResponse<List<ProductResponseDto>>(200, "Products filtered", products);
        }
        public async Task<ApiResponse<List<ProductResponseDto>>> SearchAsync(ProductSearchDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Search))
                return new ApiResponse<List<ProductResponseDto>>(400, "Search keyword is required");

            var query = _context.Products
                .Include(p => p.Category)
                .Where(p =>
                    p.IsActive &&
                    (p.Name.Contains(dto.Search) || p.Description.Contains(dto.Search))
                );

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    InStock = p.Stock > 0,
                    Category = p.Category!.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return new ApiResponse<List<ProductResponseDto>>(200, "Search results", products);
        }


    }
}
