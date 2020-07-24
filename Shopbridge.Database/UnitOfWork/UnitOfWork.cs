namespace Shopbridge.Database.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shopbridge.Database.Repository;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopBridgeDbContext context;
        private bool disposed;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(ShopBridgeDbContext context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public ITransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(context);
        }

        public void SetRepository<T>(IRepository<T> repository) where T : class
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), repository);
            }
        }

        public Repository<T> Repository<T>() where T : class
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(typeof(T), repositoryInstance);
            }
            return (Repository<T>)repositories[typeof(T)];
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                context.Dispose();
            }
            disposed = true;
        }
    }
}
