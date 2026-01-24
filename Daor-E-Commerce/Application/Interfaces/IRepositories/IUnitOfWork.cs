using Daor_E_Commerce.Application.Interfaces.IRepositories;
using Daor_E_Commerce.Application.Interfaces.Repositories;
using Daor_E_Commerce.Infrastructure.Data;

namespace Daor_E_Commerce.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }

        Task<int> SaveAsync();
    }
}
