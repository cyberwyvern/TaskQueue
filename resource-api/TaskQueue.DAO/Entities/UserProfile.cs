using System.ComponentModel.DataAnnotations;
using TaskQueue.DAO.Interface;

namespace TaskQueue.DAO.Entities
{
    public class UserProfile : IEntity<int>
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(64)]
        public string Username { get; set; }
    }
}