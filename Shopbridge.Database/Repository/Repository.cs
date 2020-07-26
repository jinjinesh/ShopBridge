namespace Shopbridge.Database.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ShopBridgeDbContext context;
        private readonly DbSet<T> entities;

        public Repository(ShopBridgeDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public ValueTask<T> FindAsync(object id)
        {
            return entities.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return entities;
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return entities.FirstOrDefaultAsync(predicate);
        }

        public ValueTask<EntityEntry<T>> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return entities.AddAsync(entity);
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
            entities.Remove(entity);
        }

        public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    }
}
