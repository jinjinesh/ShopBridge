namespace Shopbridge.Database.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public interface IRepository<T> where T : class
    {
        bool Any();

        Task<bool> AnyAsync();

        T Find(object id);

        ValueTask<T> FindAsync(object id);

        T Find(object id, params Expression<Func<T, object>>[] includeSpecs);

        ValueTask<T> FindAsync(object id, params Expression<Func<T, object>>[] includeSpecs);

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeSpecs);

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeSpecs);

        T Get(Expression<Func<T, bool>> predicate);

        T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeSpecs);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetAsyncAsNoTracking(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeSpecs);

        long? Max(Expression<Func<T, long?>> column);

        int? Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> column);

        Task<int?> MaxAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> column);

        void Insert(T entity);

        void InsertAll(ICollection<T> range);

        Task InsertAllAsync(ICollection<T> range);

        ValueTask<EntityEntry<T>> InsertAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        void RemoveRange(IEnumerable<T> entity);

        void RemoveRange(Expression<Func<T, bool>> predicate);

        void Remove(object id);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
