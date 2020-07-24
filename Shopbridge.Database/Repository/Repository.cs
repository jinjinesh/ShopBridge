namespace Shopbridge.Database.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ShopBridgeDbContext context;
        private DbSet<T> Entities;

        public Repository(ShopBridgeDbContext context)
        {
            this.context = context;
            Entities = context.Set<T>();
        }

        public bool Any()
        {
            return Entities.Any();
        }

        public Task<bool> AnyAsync()
        {
            return Entities.AnyAsync();
        }

        public T Find(object id)
        {
            return Entities.Find(id);
        }

        public T Find(object id, params Expression<Func<T, object>>[] includeSpecs)
        {
            includeSpecs.ToList().ForEach(x => Entities.Include(x).Load());
            return Entities.Find(id);
        }

        public ValueTask<T> FindAsync(object id)
        {
            return Entities.FindAsync(id);
        }

        public ValueTask<T> FindAsync(object id, params Expression<Func<T, object>>[] includeSpecs)
        {
            includeSpecs.ToList().ForEach(x => Entities.Include(x).Load());
            return Entities.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return Entities;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeSpecs)
        {
            includeSpecs.ToList().ForEach(x => Entities.Include(x).Load());
            return Entities;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeSpecs)
        {
            includeSpecs.ToList().ForEach(x => Entities.Where(predicate).Include(x).Load());
            return Entities.Where(predicate);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeSpecs)
        {
            includeSpecs.ToList().ForEach(x => Entities.Where(predicate).Include(x).Load());
            return Entities.FirstOrDefault(predicate);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return Entities.FirstOrDefaultAsync(predicate);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeSpecs)
        {
            includeSpecs.ToList().ForEach(x => Entities.Where(predicate).Include(x).Load());
            return Entities.FirstOrDefaultAsync(predicate);
        }

        public Task<T> GetAsyncAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return Entities.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public long? Max(Expression<Func<T, long?>> column)
        {
            return Entities.Max(column);
        }

        public int? Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> column)
        {
            return Entities.Where(predicate)?.Max(column);
        }

        public Task<int?> MaxAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> column)
        {
            return Entities.Where(predicate)?.MaxAsync(column);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            Entities.Add(entity);
        }

        public ValueTask<EntityEntry<T>> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Entities.AddAsync(entity);
        }

        public void InsertAll(ICollection<T> range)
        {
            if (range == null || range.Count < 1)
            {
                throw new ArgumentNullException(nameof(range));
            }
            Entities.AddRange(range);
        }

        public Task InsertAllAsync(ICollection<T> range)
        {
            if (range == null || range.Count < 1)
            {
                throw new ArgumentNullException(nameof(range));
            }

            return Entities.AddRangeAsync(range);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            Entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            Entities.RemoveRange(entity);
        }

        public void RemoveRange(Expression<Func<T, bool>> predicate)
        {
            Entities.RemoveRange(GetAll(predicate));
        }

        public void Remove(object id)
        {
            Entities.Remove(Find(id));
        }

        public void SaveChanges()
        => context.SaveChanges();

        public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    }
}
