using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TaskQueue.Authentication.DAO.DAO;
using TaskQueue.Authentication.DAO.Interface;

namespace TaskQueue.Authentication.DAO
{
    public sealed class DatabaseUnitOfWork : IDatabaseUnitOfWork
    {
        private readonly ApplicationDBContext context;
        private IDbContextTransaction transaction;

        private IUserDAO users;

        public string ConnectionString { get; private set; }

        public IUserDAO Users => users ??= new UserDAO(context);

        public DatabaseUnitOfWork(ApplicationDBContext context)
        {
            this.context = context;
            ConnectionString = this.context.Database.GetDbConnection().ConnectionString;
        }

        public void StartTransaction()
        {
            transaction ??= context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            context.SaveChanges();
            transaction?.Commit();
            transaction?.Dispose();
        }

        public void Rollback()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }

            transaction?.Rollback();
            transaction?.Dispose();
        }

        public void Dispose()
        {
            context.Dispose();
            transaction?.Dispose();
        }
    }
}