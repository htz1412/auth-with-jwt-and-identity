using AuthImplementation.Entities;
using AuthImplementation.Services;
using AuthImplementation.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthImplementation.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
             services.AddCors(options =>
             {
                 options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
             });

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseMySql(configuration["ConnectionStrings:MyDb"], 
                    ServerVersion.AutoDetect(configuration["ConnectionStrings:MyDb"]));
            });

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
