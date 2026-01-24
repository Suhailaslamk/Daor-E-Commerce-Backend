using System.Linq.Expressions;

namespace Daor_E_Commerce.Application.Interfaces.IRepositories
{
   

   
    
        public interface IGenericRepository<T> where T : class
        {
            Task<T?> GetByIdAsync(int id);
            Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

            Task AddAsync(T entity);
            void Update(T entity);
            void Delete(T entity);
        }
    }
