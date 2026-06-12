

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Title).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Content).HasMaxLength(5000).IsRequired();
            builder.HasIndex(d => new { d.CourseId, d.Type });
            builder.HasOne(d => d.Course).WithMany().HasForeignKey(d => d.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(d => d.Lesson).WithMany().HasForeignKey(d => d.LessonId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(d => d.Replies).WithOne(dr => dr.Discussion).HasForeignKey(dr => dr.DiscussionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.Votes).WithOne(dv => dv.Discussion).HasForeignKey(dv => dv.DiscussionId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
