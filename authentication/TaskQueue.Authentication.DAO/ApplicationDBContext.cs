using Microsoft.EntityFrameworkCore;
using TaskQueue.Authentication.DAO.Entities;

namespace TaskQueue.Authentication.DAO
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}