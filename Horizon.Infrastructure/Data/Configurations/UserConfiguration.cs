

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
            builder.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();
            builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.AvatarUrl).HasMaxLength(500);
            builder.Property(u => u.Bio).HasMaxLength(2000);
            builder.Property(u => u.Headline).HasMaxLength(200);
            builder.Property(u => u.Website).HasMaxLength(300);
            builder.Property(u => u.Twitter).HasMaxLength(200);
            builder.Property(u => u.LinkedIn).HasMaxLength(200);
            builder.Property(u => u.YouTube).HasMaxLength(200);

            builder.Ignore(u => u.FullName);

            // All Restrict to avoid cascade cycles — soft delete handles cleanup
            builder.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Enrollments).WithOne(e => e.Student).HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.CoursesAsInstructor).WithOne(c => c.Instructor).HasForeignKey(c => c.InstructorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Reviews).WithOne(r => r.Student).HasForeignKey(r => r.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Notifications).WithOne(n => n.Recipient).HasForeignKey(n => n.RecipientId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Discussions).WithOne(d => d.User).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.DiscussionReplies).WithOne(dr => dr.User).HasForeignKey(dr => dr.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Certificates).WithOne(c => c.Student).HasForeignKey(c => c.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Payments).WithOne(p => p.User).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Subscribers).WithOne(s => s.Instructor).HasForeignKey(s => s.InstructorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Subscriptions).WithOne(s => s.Subscriber).HasForeignKey(s => s.SubscriberId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.NotificationPreferences).WithOne(np => np.User).HasForeignKey(np => np.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.Wishlists).WithOne(w => w.User).HasForeignKey(w => w.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.CartItems).WithOne(ci => ci.User).HasForeignKey(ci => ci.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.LessonNotes).WithOne(ln => ln.User).HasForeignKey(ln => ln.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.LessonBookmarks).WithOne(lb => lb.User).HasForeignKey(lb => lb.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.CourseInstructors).WithOne(ci => ci.Instructor).HasForeignKey(ci => ci.InstructorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
