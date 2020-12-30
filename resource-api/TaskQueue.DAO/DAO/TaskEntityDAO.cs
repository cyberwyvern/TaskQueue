using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskQueue.DAO.Entities;
using TaskQueue.DAO.Interface;
using TaskQueue.DAO.Models;

namespace TaskQueue.DAO
{
    internal sealed class TaskEntityDAO : BaseDAO<TaskEntity, int>, ITaskEntityDAO
    {
        public TaskEntityDAO(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public override int Create(TaskEntity item)
        {
            var lastSequenceNumber = _db.Tasks
                .Where(t => t.UserProfileId == item.UserProfileId)
                .Max(t => (int?)t.SequenceNumber) ?? 0;

            item.SequenceNumber = lastSequenceNumber + 1;

            return base.Create(item);
        }

        public override TaskEntity Read(int id)
        {
            return _table
                .Include(t => t.UserProfile)
                .Where(t => t.Id == id)
                .SingleOrDefault();
        }

        public Page<TaskEntity> GetUserTasksPage(int pageIndex, int pageSize, int userId)
        {
            var items = _table
                .Include(t => t.UserProfile)
                .Where(i => i.UserProfile.UserId == userId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            var total = _table.Where(i => i.UserProfile.UserId == userId).Count();

            return new Page<TaskEntity>(items, pageIndex, pageSize, total);
        }
    }
}