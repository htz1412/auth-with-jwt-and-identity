using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthImplementation.Configurations
{
    public class RoleConfigurator : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            builder.HasData(
                new IdentityRole<int> { 
                    Id= 1,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                },
                new IdentityRole<int>
                {
                    Id= 2,
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                }
            );
        }
    }
}
