using Daor_E_Commerce.Application.DTOs.Admin;
using Daor_E_Commerce.Application.DTOs.Admin.Product;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Application.Services.Admin
{
    public class AdminProductService : IAdminProductService
    {
        private readonly AppDbContext _context;

        public AdminProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Create(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                Description = dto.Description,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(201, "Product created");
        }

        public async Task<ApiResponse<string>> Update(UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return new ApiResponse<string>(404, "Product not found");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Description = dto.Description;

            await _context.SaveChangesAsync();
            return new ApiResponse<string>(200, "Product updated");
        }

        public async Task<ApiResponse<string>> ToggleStatus(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new ApiResponse<string>(404, "Product not found");

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Product status updated");
        }

        public async Task<ApiResponse<string>> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new ApiResponse<string>(404, "Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Product deleted");
        }

        public async Task<ApiResponse<object>> GetAll(string? search, int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Name.Contains(search));

            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Price,
                    x.Stock,
                    x.IsActive
                })
                .ToListAsync();

            return new ApiResponse<object>(200, "Products fetched", new
            {
                total,
                page,
                pageSize,
                data
            });
        }
    }
}
