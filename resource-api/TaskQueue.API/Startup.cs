using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaskQueue.API.Middleware;
using TaskQueue.API.StartupConfig;
using TaskQueue.API.Services;
using TaskQueue.API.Hubs;

namespace TaskQueue.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var origins = Configuration.GetSection("AllowedOrigins").Get<string[]>();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }));

            services.AddDataAccessServices(Configuration);
            services.AddJwtAuthentication(Configuration);
            services.AddAuthorization();
            services.AddMapping();
            services.AddSignalR();
            services.AddSingleton<TaskQueuePublisherService>();
            services.AddHostedService<TaskQueueConsumerSevice>();

            services.AddControllers();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<QueryStringAuthMiddleware>();
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TaskHub>("/api/task-hub");
                endpoints.MapControllers();
            });
        }
    }
}