namespace Shopbridge.Database.UnitOfWork
{
    using System;
    using System.Threading.Tasks;


    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopBridgeDbContext context;
        private bool disposed;

        public UnitOfWork(ShopBridgeDbContext context)
        {
            this.context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public ITransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(context);
        }

        public void Dispose()
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
