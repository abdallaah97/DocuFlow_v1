using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Email).HasMaxLength(256);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasData(new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@docuflow.com",
                Password = "$2a$11$OI.0M2c57E8LK8SdNN1Ip.JeclkB2lB4cLQACBpqfTL5.SEHFHPkq",
                RoleId = 1,
                TenantId = 1
            });
        }
    }
}
