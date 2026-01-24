using Daor_E_Commerce.Application.Interfaces.Repositories;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product?> GetActiveByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.IsActive &&
                    !p.IsDeleted);
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> HasStockAsync(int productId, int quantity)
        {
            return await _context.Products
                .AnyAsync(p =>
                    p.Id == productId &&
                    p.Stock >= quantity &&
                    p.IsActive &&
                    !p.IsDeleted);
        }
    }
}
