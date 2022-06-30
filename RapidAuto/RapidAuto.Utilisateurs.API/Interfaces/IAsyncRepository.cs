using RapidAuto.Utilisateurs.API.Models;
using System.Linq.Expressions;

namespace RapidAuto.Utilisateurs.API.Interfaces
{
    public interface IAsyncRepository<T> where T : Utilisateur
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAsync();
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task EditAsync(T entity);
    }
}
