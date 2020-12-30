using System;

namespace TaskQueue.DAO.Interface
{
    public interface IDatabaseUnitOfWork : IDisposable
    {
        public string ConnectionString { get; }

        ITaskEntityDAO Tasks { get; }
        IUserProfileDAO UserProfiles { get; }

        void StartTransaction();

        void Commit();

        void Rollback();
    }
}