
using Daor_E_Commerce.Application.Interfaces.IRepositories;
using Daor_E_Commerce.Domain.Entities;

namespace Daor_E_Commerce.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
