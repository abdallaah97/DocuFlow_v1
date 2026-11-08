using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            //Seed data for the Tenant entity
            builder.HasData(
                new Tenant
                {
                    Id = 1,
                    Name = "Default Tenant",
                    Description = "This is the default tenant.",
                    Code = "DEFAULT",
                    IsActive = true,
                    IsDeleted = false
                }
            );

        }
    }
}
