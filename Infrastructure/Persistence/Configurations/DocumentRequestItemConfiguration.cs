using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DocumentRequestItemConfiguration : IEntityTypeConfiguration<DocumentRequestItem>
    {
        public void Configure(EntityTypeBuilder<DocumentRequestItem> builder)
        {
            builder.ToTable("DocumentRequestItems");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(t => t.FileUrl)
               .HasMaxLength(500);

            builder.Property(t => t.RejectReason)
               .HasMaxLength(1000);

            builder.Property(t => t.Description)
              .IsRequired()
              .HasMaxLength(5000);

            builder.HasOne(x => x.DocumentRequest)
                .WithMany(x => x.DocumentRequestItems)
                .HasForeignKey(x => x.DocumentRequestId);
        }
    }
}
