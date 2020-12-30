using System.ComponentModel.DataAnnotations;
using TaskQueue.Authentication.DAO.Interface;

namespace TaskQueue.Authentication.DAO.Entities
{
    public class User : IEntity<int>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Username { get; set; }

        [Required]
        [StringLength(64)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(64)]
        public string Salt { get; set; }
    }
}