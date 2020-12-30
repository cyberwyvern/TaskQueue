using TaskQueue.Authentication.DAO.Entities;

namespace TaskQueue.Authentication.DAO.Interface
{
    public interface IUserDAO : IBaseDAO<User, int>
    {
        User GetByUsername(string username);
    }
}