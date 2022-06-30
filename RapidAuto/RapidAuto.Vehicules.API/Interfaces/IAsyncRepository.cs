using RapidAuto.Vehicules.API.Models;
using System.Linq.Expressions;

namespace RapidAuto.Vehicules.API.Interfaces
{
    public interface IAsyncRepository<T> where T : Vehicule
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAsync();
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task EditAsync(T entity);
    }
}
