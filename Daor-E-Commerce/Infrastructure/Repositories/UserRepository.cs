//namespace Daor_E_Commerce.Infrastructure.Repositories
//{
//    using Daor_E_Commerce.Application.Interfaces.IRepositories;
//    using Daor_E_Commerce.Domain.Entities;
//    using Microsoft.EntityFrameworkCore;

//    public class UserRepository : IUserRepository
//    {
//        private readonly AppDbContext _context;

//        public UserRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<User?> GetByIdAsync(int id)
//        {
//            return await _context.Users
//                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
//        }

//        public async Task<User?> GetByEmailAsync(string email)
//        {
//            return await _context.Users
//                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
//        }

//        public async Task<bool> EmailExistsAsync(string email)
//        {
//            return await _context.Users
//                .AnyAsync(u => u.Email == email && !u.IsDeleted);
//        }

//        public async Task AddAsync(User user)
//        {
//            await _context.Users.AddAsync(user);
//            await _context.SaveChangesAsync();
//        }

//        public void Update(User user)
//        {
//            _context.Users.Update(user);
//            _context.SaveChanges();
//        }
//    }

//}
using Daor_E_Commerce.Application.Interfaces.Repositories;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email && !x.IsDeleted);
        }
    }
}
