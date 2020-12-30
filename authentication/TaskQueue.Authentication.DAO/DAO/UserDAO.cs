using System.Linq;
using TaskQueue.Authentication.DAO.Entities;
using TaskQueue.Authentication.DAO.Interface;

namespace TaskQueue.Authentication.DAO.DAO
{
    internal sealed class UserDAO : BaseDAO<User, int>, IUserDAO
    {
        public UserDAO(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public User GetByUsername(string username)
        {
            return _table.SingleOrDefault(u => u.Username == username);
        }
    }
}