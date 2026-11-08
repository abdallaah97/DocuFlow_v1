using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AccessLinkConfiguration : IEntityTypeConfiguration<AccessLink>
    {
        public void Configure(EntityTypeBuilder<AccessLink> builder)
        {
            builder.ToTable("AccessLinks");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Token).HasMaxLength(128);
            builder.HasIndex(x => x.Token).IsUnique();

            builder.HasOne(x => x.DocumentRequest)
                .WithMany(x => x.AccessLinks)
                .HasForeignKey(x => x.DocumentRequestId);
        }
    }
}
