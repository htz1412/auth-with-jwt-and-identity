using AuthImplementation.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthImplementation.Extensions
{
    public static class IdentityConfigurator
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 10;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
