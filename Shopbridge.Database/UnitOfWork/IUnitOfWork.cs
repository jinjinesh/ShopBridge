namespace Shopbridge.Database.UnitOfWork
{
    using System;
    using System.Threading.Tasks;

    using Shopbridge.Database.Repository;

    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        ITransaction BeginTransaction();
    }
}
