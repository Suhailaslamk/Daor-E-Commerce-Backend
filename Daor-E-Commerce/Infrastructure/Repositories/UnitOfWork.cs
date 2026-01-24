using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Application.Interfaces.Repositories;
using Daor_E_Commerce.Infrastructure.Data;

namespace Daor_E_Commerce.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public ICartRepository Carts { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository users,
            IProductRepository products,
            ICartRepository carts,
            IOrderRepository orders)
        {
            _context = context;
            Users = users;
            Products = products;
            Carts = carts;
            Orders = orders;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
