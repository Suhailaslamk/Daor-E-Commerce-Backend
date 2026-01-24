using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        private readonly ICloudinaryService _cloudinary;


        public AdminProductService(AppDbContext context, ICloudinaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;

        }

        public async Task<ApiResponse<ProductResponseDto>> Create(CreateProductDto dto)
        {
            if (dto.ImageFile == null)
                return new ApiResponse<ProductResponseDto>(400, "Image file is required");

            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return new ApiResponse<ProductResponseDto>(404, "Category not found");

            var normalizedName = dto.Name.Trim().ToLower().Replace(" ", "");

            // Check duplicates
            bool exists = await _context.Products
                .AnyAsync(p => p.NormalizedName == normalizedName && !p.IsDeleted);

            if (exists)
                return new ApiResponse<ProductResponseDto>(409, "Product with same name already exists");
            var uploadResult = await _cloudinary.UploadImageAsync(dto.ImageFile);
            var product = new Product
            {
                Name = dto.Name.Trim(),
                NormalizedName = normalizedName, // ✅ add this
                Price = dto.Price,
                Stock = dto.Stock,
                Description = dto.Description.Trim(),
                ImageUrl = uploadResult.Url,
                IsActive = true,
                CategoryId = dto.CategoryId
            };

            

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };

            return new ApiResponse<ProductResponseDto>(
                201,
                "Product created successfully",
                response
            );
        }


        public async Task<ApiResponse<string>> PatchUpdate(int id, UpdateProductPatchDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new ApiResponse<string>(404, "Product not found");

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                var normalizedName = dto.Name.Trim().ToLower().Replace(" ", "");

                bool exists = await _context.Products
                    .AnyAsync(p => p.NormalizedName == normalizedName && p.Id != product.Id && !p.IsDeleted);

                if (exists)
                    return new ApiResponse<string>(409, "Another product with same name exists");

                product.Name = dto.Name.Trim();
                product.NormalizedName = normalizedName; // ✅
            }
            if (!string.IsNullOrWhiteSpace(dto.Description))
                product.Description = dto.Description.Trim();
            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;
            if (dto.Stock.HasValue)
                product.Stock = dto.Stock.Value;
            if (dto.CategoryId.HasValue)
            {
                var category = await _context.Categories.FindAsync(dto.CategoryId.Value);
                if (category == null)
                    return new ApiResponse<string>(404, "Category not found");
                product.CategoryId = dto.CategoryId.Value;
            }

            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrWhiteSpace(product.ImagePublicId))
                {
                    await _cloudinary.DeleteImageAsync(product.ImagePublicId); // ✅ delete old
                }

                var uploadResult = await _cloudinary.UploadImageAsync(dto.ImageFile);

                product.ImageUrl = uploadResult.Url;
                product.ImagePublicId = uploadResult.PublicId; // store ID for future cleanup
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Product updated successfully");
        }
        public async Task<ApiResponse<ProductResponseDto>> GetById(int id)
        {
            var query = _context.Products.AsNoTracking().Where(p => !p.IsDeleted);
            var product = await _context.Products
                   .AsNoTracking()
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null)
                return new ApiResponse<ProductResponseDto>(404, "Product not found");

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };

            return new ApiResponse<ProductResponseDto>(200, "Product fetched successfully", response);
        }

        public async Task<ApiResponse<string>> ToggleStatus(int productId)
        {

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);

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

            product.IsDeleted = true; 
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Product deleted");
        }

        public async Task<ApiResponse<PagedResponse<ProductListItemDto>>> GetAll(string? search, int page, int pageSize)
        {
            var query = _context.Products.AsNoTracking().Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = search.Trim().ToLower().Replace(" ", "");
                query = query.Where(x => x.NormalizedName.Contains(normalizedSearch));
            }

            var total = await query.CountAsync();

            var data = await query
    .OrderByDescending(x => x.Id)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .Select(x => new ProductListItemDto
    {
        Id = x.Id,
        Name = x.Name,
        Price = x.Price,
        Stock = x.Stock,
        IsActive = x.IsActive
    })
    .ToListAsync();

            return new ApiResponse<PagedResponse<ProductListItemDto>>(200, "Products fetched", new PagedResponse<ProductListItemDto>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Data = data
            });
        }
    }
}
