using Microsoft.EntityFrameworkCore;
using TaskQueue.DAO.Entities;

namespace TaskQueue.DAO
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }

        public DbSet<UserProfile> UsersProfiles { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}