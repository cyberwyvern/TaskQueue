using System;

namespace TaskQueue.Authentication.DAO.Interface
{
    public interface IDatabaseUnitOfWork : IDisposable
    {
        public string ConnectionString { get; }

        IUserDAO Users { get; }

        void StartTransaction();

        void Commit();

        void Rollback();
    }
}