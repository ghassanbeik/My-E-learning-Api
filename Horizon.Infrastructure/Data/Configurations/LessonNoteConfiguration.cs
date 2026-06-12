

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class LessonNoteConfiguration : IEntityTypeConfiguration<LessonNote>
    {
        public void Configure(EntityTypeBuilder<LessonNote> builder)
        {
            builder.HasKey(ln => ln.Id);
            builder.Property(ln => ln.Content).HasMaxLength(5000).IsRequired();
            builder.HasIndex(ln => new { ln.LessonId, ln.UserId });
        }
    }
}
