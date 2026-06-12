

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(5000).IsRequired();
            builder.Property(c => c.Subtitle).HasMaxLength(300);
            builder.Property(c => c.ShortDescription).HasMaxLength(500);
            builder.Property(c => c.Language).HasMaxLength(50);
            builder.Property(c => c.Currency).HasMaxLength(10).HasDefaultValue("USD");
            builder.Property(c => c.Price).HasPrecision(18, 2);
            builder.Property(c => c.DiscountPrice).HasPrecision(18, 2);

            builder.Ignore(c => c.HasDiscount);
            builder.Ignore(c => c.CurrentPrice);

            builder.HasIndex(c => c.Status);
            builder.HasIndex(c => c.IsFeatured);
            builder.HasIndex(c => c.AverageRating);
            builder.HasIndex(c => c.TotalStudents);
            builder.HasIndex(c => c.CreatedAt);

            builder.HasMany(c => c.Sections).WithOne(s => s.Course).HasForeignKey(s => s.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Reviews).WithOne(r => r.Course).HasForeignKey(r => r.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.Enrollments).WithOne(e => e.Course).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.Wishlists).WithOne(w => w.Course).HasForeignKey(w => w.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.CartItems).WithOne(ci => ci.Course).HasForeignKey(ci => ci.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.CourseTags).WithOne(ct => ct.Course).HasForeignKey(ct => ct.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.CourseCategories).WithOne(cc => cc.Course).HasForeignKey(cc => cc.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Announcements).WithOne(a => a.Course).HasForeignKey(a => a.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.CourseInstructors).WithOne(ci => ci.Course).HasForeignKey(ci => ci.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.LiveSessions).WithOne(ls => ls.Course).HasForeignKey(ls => ls.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.BundleCourses).WithOne(bc => bc.Course).HasForeignKey(bc => bc.CourseId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
