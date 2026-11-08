using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(t => t.Id);

            builder.HasData(
                new Role { Id = 1, Name = RoleNames.PlatformAdmin, Description = "Manages the whole platform" },
                new Role { Id = 2, Name = RoleNames.Owner, Description = "Organization owner" },
                new Role { Id = 3, Name = RoleNames.Employee, Description = "Organization employee" });
        }
    }
}
