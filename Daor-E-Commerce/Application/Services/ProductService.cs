////using Daor_E_Commerce.Application.DTOs.Products;
////using Daor_E_Commerce.Application.Interfaces;
////using Daor_E_Commerce.Common;
////using Daor_E_Commerce.Infrastructure.Data;
////using Microsoft.EntityFrameworkCore;

////namespace Daor_E_Commerce.Application.Services
////{
////    public class ProductService : IProductService
////    {
////        private readonly AppDbContext _context;

////        public ProductService(AppDbContext context)
////        {
////            _context = context;
////        }

////        public async Task<ApiResponse<object>> GetAll()
////        {
////            var products = await _context.Products
////                .Where(p => p.IsActive)
////                .Select(p => new
////                {
////                    p.Id,
////                    p.Name,
////                    p.Description,
////                    p.Price,
////                    InStock = p.Stock > 0,
////                    Category = p.Category.Name
////                }).ToListAsync();

////            return new ApiResponse<object>(200, "Products fetched", products);
////        }

////        public async Task<ApiResponse<object>> GetById(int id)
////        {
////            var product = await _context.Products
////                .Include(p => p.Category)
////                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

////            if (product == null)
////                return new ApiResponse<object>(404, "Product not found");

////            return new ApiResponse<object>(200, "Product fetched", product);
////        }

////        public async Task<ApiResponse<object>> GetByCategory(int categoryId)
////        {
////            var products = await _context.Products
////                .Where(p => p.CategoryId == categoryId && p.IsActive)
////                .ToListAsync();

////            return new ApiResponse<object>(200, "Products fetched", products);
////        }

////        public async Task<ApiResponse<object>> GetPaged(int page, int pageSize)
////        {
////            var products = await _context.Products
////                .Where(p => p.IsActive)
////                .Skip((page - 1) * pageSize)
////                .Take(pageSize)
////                .ToListAsync();

////            return new ApiResponse<object>(200, "Paged products", products);
////        }

////        public async Task<ApiResponse<object>> Search(string search, int page, int pageSize)
////        {
////            var products = await _context.Products
////                .Where(p => p.IsActive && p.Name.Contains(search))
////                .Skip((page - 1) * pageSize)
////                .Take(pageSize)
////                .ToListAsync();

////            return new ApiResponse<object>(200, "Search results", products);
////        }

////        public async Task<ApiResponse<object>> FilterAndSort(ProductFilterDto dto)
////        {
////            var query = _context.Products.AsQueryable();

////            if (dto.ProductId.HasValue)
////                query = query.Where(p => p.Id == dto.ProductId);

////            if (!string.IsNullOrEmpty(dto.Name))
////                query = query.Where(p => p.Name.Contains(dto.Name));

////            if (dto.CategoryId.HasValue)
////                query = query.Where(p => p.CategoryId == dto.CategoryId);

////            if (dto.MinPrice.HasValue)
////                query = query.Where(p => p.Price >= dto.MinPrice);

////            if (dto.MaxPrice.HasValue)
////                query = query.Where(p => p.Price <= dto.MaxPrice);

////            if (dto.InStock.HasValue)
////                query = query.Where(p => (p.Stock > 0) == dto.InStock);

////            if (dto.IsActive.HasValue)
////                query = query.Where(p => p.IsActive == dto.IsActive);

////            query = dto.SortBy?.ToLower() switch
////            {
////                "name" => dto.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
////                "createdon" => dto.Descending ? query.OrderByDescending(p => p.CreatedOn) : query.OrderBy(p => p.CreatedOn),
////                _ => dto.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price)
////            };

////            var result = await query
////                .Skip((dto.Page - 1) * dto.PageSize)
////                .Take(dto.PageSize)
////                .ToListAsync();

////            return new ApiResponse<object>(200, "Filtered products", result);
////        }
////    }
////}



//public class ProductService : IProductService
//{
//    private readonly AppDbContext _context;

//    public ProductService(AppDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<ApiResponse<object>> GetAll()
//    {
//        var products = await _context.Products
//            .Where(p => p.IsActive)
//            .Include(p => p.Category)
//            .Select(p => MapProduct(p))
//            .ToListAsync();

//        return new ApiResponse<object>(200, "Products retrieved", products);
//    }

//    public async Task<ApiResponse<object>> GetById(int id)
//    {
//        var product = await _context.Products
//            .Include(p => p.Category)
//            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

//        if (product == null)
//            return new ApiResponse<object>(404, "Product not found");

//        return new ApiResponse<object>(200, "Product details", MapProduct(product));
//    }

//    public async Task<ApiResponse<object>> GetByCategory(int categoryId)
//    {
//        var products = await _context.Products
//            .Where(p => p.CategoryId == categoryId && p.IsActive)
//            .Include(p => p.Category)
//            .Select(p => MapProduct(p))
//            .ToListAsync();

//        return new ApiResponse<object>(200, "Category products", products);
//    }

//    public async Task<ApiResponse<object>> GetPaged(int page, int pageSize)
//    {
//        var query = _context.Products
//            .Where(p => p.IsActive)
//            .Include(p => p.Category);

//        var total = await query.CountAsync();

//        var items = await query
//            .Skip((page - 1) * pageSize)
//            .Take(pageSize)
//            .Select(p => MapProduct(p))
//            .ToListAsync();

//        return new ApiResponse<object>(200, "Paged products", new
//        {
//            total,
//            page,
//            pageSize,
//            items
//        });
//    }

//    public async Task<ApiResponse<object>> FilterSort(ProductFilterDto dto)
//    {
//        var query = _context.Products
//            .Include(p => p.Category)
//            .AsQueryable();

//        if (dto.ProductId.HasValue)
//            query = query.Where(p => p.Id == dto.ProductId);

//        if (!string.IsNullOrWhiteSpace(dto.Name))
//            query = query.Where(p => p.Name.Contains(dto.Name));

//        if (dto.CategoryId.HasValue)
//            query = query.Where(p => p.CategoryId == dto.CategoryId);

//        if (dto.MinPrice.HasValue)
//            query = query.Where(p => p.Price >= dto.MinPrice);

//        if (dto.MaxPrice.HasValue)
//            query = query.Where(p => p.Price <= dto.MaxPrice);

//        if (dto.InStock.HasValue)
//            query = dto.InStock.Value
//                ? query.Where(p => p.Stock > 0)
//                : query.Where(p => p.Stock == 0);

//        if (dto.IsActive.HasValue)
//            query = query.Where(p => p.IsActive == dto.IsActive);

//        query = dto.SortBy?.ToLower() switch
//        {
//            "name" => dto.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
//            "createdon" => dto.Descending ? query.OrderByDescending(p => p.CreatedOn) : query.OrderBy(p => p.CreatedOn),
//            _ => dto.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price)
//        };

//        var total = await query.CountAsync();

//        var items = await query
//            .Skip((dto.Page - 1) * dto.PageSize)
//            .Take(dto.PageSize)
//            .Select(p => MapProduct(p))
//            .ToListAsync();

//        return new ApiResponse<object>(200, "Filtered products", new
//        {
//            total,
//            dto.Page,
//            dto.PageSize,
//            items
//        });
//    }

//    public async Task<ApiResponse<object>> Search(string search, int page, int pageSize)
//    {
//        var query = _context.Products
//            .Where(p => p.IsActive &&
//                   (p.Name.Contains(search) || p.Description!.Contains(search)))
//            .Include(p => p.Category);

//        var total = await query.CountAsync();

//        var items = await query
//            .Skip((page - 1) * pageSize)
//            .Take(pageSize)
//            .Select(p => MapProduct(p))
//            .ToListAsync();

//        return new ApiResponse<object>(200, "Search results", new
//        {
//            total,
//            page,
//            pageSize,
//            items
//        });
//    }

//    private static object MapProduct(Product p)
//    {
//        return new
//        {
//            p.Id,
//            p.Name,
//            p.Description,
//            p.Price,
//            InStock = p.Stock > 0,
//            p.ImageUrl,
//            Category = p.Category!.Name
//        };
//    }
//}


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
