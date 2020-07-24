namespace Shopbridge.Database.UnitOfWork
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class EntityDatabaseTransaction : ITransaction
    {
        private readonly IDbContextTransaction Transaction;

        public EntityDatabaseTransaction(DbContext context)
        {
            Transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Dispose()
        {
            Transaction.Dispose();
        }
    }
}
