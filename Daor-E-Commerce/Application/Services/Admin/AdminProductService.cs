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

        public async Task<ApiResponse<object>> GetAll()
        {
            var products = await _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.IsActive,
                    p.CreatedAt
                })
                .ToListAsync();

            return new ApiResponse<object>(200, "Products fetched", products);
        }

        public async Task<ApiResponse<string>> Add(AddProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(201, "Product added");
        }

        public async Task<ApiResponse<string>> Update(UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(dto.Id);
            if (product == null)
                return new ApiResponse<string>(404, "Product not found");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.IsActive = dto.IsActive;

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
    }
}
