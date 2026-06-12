
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasMany(t => t.CourseTags).WithOne(ct => ct.Tag).HasForeignKey(ct => ct.TagId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
