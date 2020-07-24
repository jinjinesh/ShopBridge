namespace Shopbridge.Database.UnitOfWork
{
    using System.Threading.Tasks;

    using Shopbridge.Database.Repository;

    public interface IUnitOfWork
    {
        void SetRepository<T>(IRepository<T> repository) where T : class;
        Repository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        void Dispose();
        ITransaction BeginTransaction();
    }
}
