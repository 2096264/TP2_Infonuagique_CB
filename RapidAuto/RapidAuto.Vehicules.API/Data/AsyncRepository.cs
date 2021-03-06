using Microsoft.EntityFrameworkCore;
using RapidAuto.Vehicules.API.Interfaces;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Data
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : Vehicule
    {
        protected readonly VehiculeDbContext _dbContext;

        public AsyncRepository(VehiculeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> ListAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }
    }
}

