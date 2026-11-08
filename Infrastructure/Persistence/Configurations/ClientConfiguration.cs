using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Description).HasMaxLength(5000);
            builder.Property(t => t.Name).HasMaxLength(100);
            builder.Property(t => t.Email).HasMaxLength(100);
            builder.Property(t => t.PhoneNumber).HasMaxLength(100);

            builder.HasOne(x => x.AssignedEmployee)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.AssignedEmployeeId);
        }
    }
}
