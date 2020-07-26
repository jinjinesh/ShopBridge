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

        public Task<bool> AnyAsync()
        {
            return Entities.AnyAsync();
        }

        public T Find(object id)
        {
            return Entities.Find(id);
        }

        public ValueTask<T> FindAsync(object id)
        {
            return Entities.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return Entities;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate);
        }
        
        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return Entities.FirstOrDefaultAsync(predicate);
        }

        public ValueTask<EntityEntry<T>> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Entities.AddAsync(entity);
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

        public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    }
}
