

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class LessonBookmarkConfiguration : IEntityTypeConfiguration<LessonBookmark>
    {
        public void Configure(EntityTypeBuilder<LessonBookmark> builder)
        {
            builder.HasKey(lb => lb.Id);
            builder.Property(lb => lb.Note).HasMaxLength(1000);
            builder.HasIndex(lb => new { lb.LessonId, lb.UserId });
        }
    }
}
