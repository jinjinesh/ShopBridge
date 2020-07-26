namespace Shopbridge.Database.UnitOfWork
{
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        ITransaction BeginTransaction();
    }
}
