

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Title).HasMaxLength(200).IsRequired();
            builder.HasMany(s => s.Lessons).WithOne(l => l.Section).HasForeignKey(l => l.SectionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
