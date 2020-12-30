using TaskQueue.DAO.Entities;

namespace TaskQueue.DAO.Interface
{
    public interface IUserProfileDAO : IBaseDAO<UserProfile, int>
    {
        UserProfile GetByUserId(int userId);
    }
}