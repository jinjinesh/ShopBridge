namespace Shopbridge.Database.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public interface IRepository<T> where T : class
    {
        ValueTask<T> FindAsync(object id);

        IQueryable<T> GetAll();

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        ValueTask<EntityEntry<T>> InsertAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();
    }
}
