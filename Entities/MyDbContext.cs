using AuthImplementation.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthImplementation.Entities
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfigurator());
            base.OnModelCreating(builder);
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
