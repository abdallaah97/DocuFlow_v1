using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DocumentRequestConfiguration : IEntityTypeConfiguration<DocumentRequest>
    {
        public void Configure(EntityTypeBuilder<DocumentRequest> builder)
        {
            builder.ToTable("DocumentRequests");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(t => t.Description)
              .IsRequired()
              .HasMaxLength(5000);

            builder.HasOne(x => x.Client)
                .WithMany(x => x.DocumentRequests)
                .HasForeignKey(x => x.ClientId);
        }
    }
}
