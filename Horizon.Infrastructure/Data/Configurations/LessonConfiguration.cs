

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Title).HasMaxLength(200).IsRequired();
            builder.Property(l => l.VideoUrl).HasMaxLength(500);
            builder.Property(l => l.ResourceUrl).HasMaxLength(500);
            builder.HasMany(l => l.Progresses).WithOne(p => p.Lesson).HasForeignKey(p => p.LessonId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(l => l.Quizzes).WithOne(q => q.Lesson).HasForeignKey(q => q.LessonId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(l => l.Assignments).WithOne(a => a.Lesson).HasForeignKey(a => a.LessonId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(l => l.Notes).WithOne(n => n.Lesson).HasForeignKey(n => n.LessonId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(l => l.Bookmarks).WithOne(b => b.Lesson).HasForeignKey(b => b.LessonId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
