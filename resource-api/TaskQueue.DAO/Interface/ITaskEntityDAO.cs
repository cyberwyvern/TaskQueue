using TaskQueue.DAO.Entities;
using TaskQueue.DAO.Models;

namespace TaskQueue.DAO.Interface
{
    public interface ITaskEntityDAO : IBaseDAO<TaskEntity, int>
    {
        Page<TaskEntity> GetUserTasksPage(int pageIndex, int pageSize, int userId);
    }
}