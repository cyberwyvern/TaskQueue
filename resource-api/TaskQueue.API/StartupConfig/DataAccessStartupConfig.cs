using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TaskQueue.DAO;
using TaskQueue.DAO.Interface;

namespace TaskQueue.API.StartupConfig
{
    internal static class DataAccessStartupConfig
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                if (Debugger.IsAttached)
                {
                    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
                }

                options.UseSqlServer(config.GetConnectionString("DBconnection"));
            });

            services.AddScoped<IDatabaseUnitOfWork, DatabaseUnitOfWork>();
        }
    }
}