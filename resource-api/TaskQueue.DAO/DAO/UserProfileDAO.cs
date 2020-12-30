using System.Linq;
using TaskQueue.DAO.Entities;
using TaskQueue.DAO.Interface;

namespace TaskQueue.DAO
{
    class UserProfileDAO : BaseDAO<UserProfile, int>, IUserProfileDAO
    {
        public UserProfileDAO(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public UserProfile GetByUserId(int userId)
        {
            return _table.SingleOrDefault(p => p.UserId == userId);
        }
    }
}