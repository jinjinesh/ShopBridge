namespace Shopbridge.Database.UnitOfWork
{
    using System;

    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
