using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Horizon.Domain.Entities;

namespace Horizon.Infrastructure.Data.Configurations;

// ─── Identity & Auth ────────────────────────────────────────────────────────

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

public class RoleConfiguration : IEntityTypeConfiguration<RoleInfo>
{
    public void Configure(EntityTypeBuilder<RoleInfo> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        builder.HasIndex(r => r.Name).IsUnique();
        builder.HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(r => r.RolePermissions).WithOne(rp => rp.Role).HasForeignKey(rp => rp.RoleId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Resource).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Action).HasMaxLength(50).IsRequired();
        builder.HasMany(p => p.RolePermissions).WithOne(rp => rp.Permission).HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
    }
}

public class InstructorProfileConfiguration : IEntityTypeConfiguration<InstructorProfile>
{
    public void Configure(EntityTypeBuilder<InstructorProfile> builder)
    {
        builder.HasKey(ip => ip.Id);
        builder.Property(ip => ip.AverageRating).HasPrecision(5, 2);
        builder.Property(ip => ip.TotalEarnings).HasPrecision(18, 2);
        builder.Property(ip => ip.PendingEarnings).HasPrecision(18, 2);
        builder.HasOne(ip => ip.User).WithMany(u => u.InstructorProfiles).HasForeignKey(ip => ip.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class InstructorSubscriberConfiguration : IEntityTypeConfiguration<InstructorSubscriber>
{
    public void Configure(EntityTypeBuilder<InstructorSubscriber> builder)
    {
        builder.HasKey(s => new { s.InstructorId, s.SubscriberId });
        builder.HasOne(s => s.Instructor).WithMany(u => u.Subscribers).HasForeignKey(s => s.InstructorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Subscriber).WithMany(u => u.Subscriptions).HasForeignKey(s => s.SubscriberId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.RefreshToken).HasMaxLength(500).IsRequired();
        builder.HasIndex(s => s.RefreshToken).IsUnique();
        builder.HasIndex(s => new { s.UserId, s.RevokedAt });
        builder.Ignore(s => s.IsActive);
        builder.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── Catalog ────────────────────────────────────────────────────────────────

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Color).HasMaxLength(50);
        builder.HasIndex(c => c.Name).IsUnique();
        builder.HasIndex(c => c.IsFeatured);
        builder.HasOne(c => c.Parent).WithMany(c => c.SubCategories).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Restrict);
    }
}

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

public class CourseInstructorConfiguration : IEntityTypeConfiguration<CourseInstructor>
{
    public void Configure(EntityTypeBuilder<CourseInstructor> builder)
    {
        builder.HasKey(ci => ci.Id);
        builder.HasIndex(ci => new { ci.CourseId, ci.InstructorId }).IsUnique();
        builder.Property(ci => ci.RevenueSharePercentage).HasPrecision(5, 2);
    }
}

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).HasMaxLength(200).IsRequired();
        builder.HasMany(s => s.Lessons).WithOne(l => l.Section).HasForeignKey(l => l.SectionId).OnDelete(DeleteBehavior.Cascade);
    }
}

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

public class LessonNoteConfiguration : IEntityTypeConfiguration<LessonNote>
{
    public void Configure(EntityTypeBuilder<LessonNote> builder)
    {
        builder.HasKey(ln => ln.Id);
        builder.Property(ln => ln.Content).HasMaxLength(5000).IsRequired();
        builder.HasIndex(ln => new { ln.LessonId, ln.UserId });
    }
}

public class LessonBookmarkConfiguration : IEntityTypeConfiguration<LessonBookmark>
{
    public void Configure(EntityTypeBuilder<LessonBookmark> builder)
    {
        builder.HasKey(lb => lb.Id);
        builder.Property(lb => lb.Note).HasMaxLength(1000);
        builder.HasIndex(lb => new { lb.LessonId, lb.UserId });
    }
}

public class LiveSessionConfiguration : IEntityTypeConfiguration<LiveSession>
{
    public void Configure(EntityTypeBuilder<LiveSession> builder)
    {
        builder.HasKey(ls => ls.Id);
        builder.Property(ls => ls.Title).HasMaxLength(200).IsRequired();
        builder.Property(ls => ls.MeetingUrl).HasMaxLength(500);
        builder.Property(ls => ls.RecordingUrl).HasMaxLength(500);
        builder.HasIndex(ls => ls.ScheduledAt);
        builder.HasOne(ls => ls.Instructor).WithMany().HasForeignKey(ls => ls.InstructorId).OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── Learning ───────────────────────────────────────────────────────────────

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AmountPaid).HasPrecision(18, 2);
        builder.Property(e => e.DiscountApplied).HasPrecision(18, 2);
        builder.Property(e => e.ProgressPercentage).HasPrecision(5, 2);
        builder.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
        builder.HasIndex(e => e.Status);
        builder.HasMany(e => e.Progresses).WithOne(p => p.Enrollment).HasForeignKey(p => p.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Certificates).WithOne(c => c.Enrollment).HasForeignKey(c => c.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Payments).WithOne(p => p.Enrollment).HasForeignKey(p => p.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProgressConfiguration : IEntityTypeConfiguration<Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.EnrollmentId, p.LessonId }).IsUnique();
    }
}

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CertificateNumber).HasMaxLength(100).IsRequired();
        builder.Property(c => c.VerificationUrl).HasMaxLength(500);
        builder.HasIndex(c => c.CertificateNumber).IsUnique();
        builder.HasOne(c => c.Course).WithMany().HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Title).HasMaxLength(200).IsRequired();
        builder.HasMany(q => q.Questions).WithOne(q => q.Quiz).HasForeignKey(q => q.QuizId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(q => q.Attempts).WithOne(a => a.Quiz).HasForeignKey(a => a.QuizId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Text).HasMaxLength(1000).IsRequired();
        builder.HasMany(q => q.AnswerOptions).WithOne(a => a.Question).HasForeignKey(a => a.QuestionId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.HasKey(qa => qa.Id);
        builder.HasOne(qa => qa.Student).WithMany().HasForeignKey(qa => qa.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(qa => qa.Answers).WithOne(a => a.Attempt).HasForeignKey(a => a.AttemptId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAnswer> builder)
    {
        builder.HasKey(qa => qa.Id);
        builder.HasOne(qa => qa.Question).WithMany().HasForeignKey(qa => qa.QuestionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(qa => qa.SelectedAnswer).WithMany().HasForeignKey(qa => qa.SelectedAnswerId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Description).HasMaxLength(5000).IsRequired();
        builder.HasMany(a => a.Submissions).WithOne(s => s.Assignment).HasForeignKey(s => s.AssignmentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssignmentSubmissionConfiguration : IEntityTypeConfiguration<AssignmentSubmission>
{
    public void Configure(EntityTypeBuilder<AssignmentSubmission> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Student).WithMany().HasForeignKey(a => a.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.GradedBy).WithMany().HasForeignKey(a => a.GradedById).OnDelete(DeleteBehavior.Restrict);
    }
}

// ─── Engagement ─────────────────────────────────────────────────────────────

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Comment).HasMaxLength(2000);
        builder.HasIndex(r => new { r.StudentId, r.CourseId }).IsUnique();
        builder.HasIndex(r => r.Status);
        builder.HasMany(r => r.Responses).WithOne(rr => rr.Review).HasForeignKey(rr => rr.ReviewId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(r => r.Votes).WithOne(rv => rv.Review).HasForeignKey(rv => rv.ReviewId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ReviewResponseConfiguration : IEntityTypeConfiguration<ReviewResponse>
{
    public void Configure(EntityTypeBuilder<ReviewResponse> builder)
    {
        builder.HasKey(rr => rr.Id);
        builder.HasOne(rr => rr.Responder).WithMany().HasForeignKey(rr => rr.ResponderId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ReviewVoteConfiguration : IEntityTypeConfiguration<ReviewVote>
{
    public void Configure(EntityTypeBuilder<ReviewVote> builder)
    {
        builder.HasKey(rv => rv.Id);
        builder.HasIndex(rv => new { rv.ReviewId, rv.UserId }).IsUnique();
        builder.HasOne(rv => rv.User).WithMany().HasForeignKey(rv => rv.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

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

public class DiscussionReplyConfiguration : IEntityTypeConfiguration<DiscussionReply>
{
    public void Configure(EntityTypeBuilder<DiscussionReply> builder)
    {
        builder.HasKey(dr => dr.Id);
        builder.Property(dr => dr.Content).HasMaxLength(5000).IsRequired();
        builder.HasOne(dr => dr.ParentReply).WithMany(dr => dr.ChildReplies).HasForeignKey(dr => dr.ParentReplyId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(dr => dr.Votes).WithOne(dv => dv.Reply).HasForeignKey(dv => dv.ReplyId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class DiscussionVoteConfiguration : IEntityTypeConfiguration<DiscussionVote>
{
    public void Configure(EntityTypeBuilder<DiscussionVote> builder)
    {
        builder.HasKey(dv => dv.Id);
        builder.HasOne(dv => dv.User).WithMany().HasForeignKey(dv => dv.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Content).HasMaxLength(5000).IsRequired();
        builder.HasOne(a => a.Instructor).WithMany().HasForeignKey(a => a.InstructorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(a => a.ReadBy).WithOne(ar => ar.Announcement).HasForeignKey(ar => ar.AnnouncementId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class AnnouncementReadConfiguration : IEntityTypeConfiguration<AnnouncementRead>
{
    public void Configure(EntityTypeBuilder<AnnouncementRead> builder)
    {
        builder.HasKey(ar => new { ar.AnnouncementId, ar.UserId });
        builder.HasOne(ar => ar.Announcement).WithMany(a => a.ReadBy).HasForeignKey(ar => ar.AnnouncementId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ar => ar.User).WithMany().HasForeignKey(ar => ar.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
        builder.Property(n => n.Message).HasMaxLength(2000).IsRequired();
        builder.Property(n => n.ActionUrl).HasMaxLength(500);
        builder.Property(n => n.ImageUrl).HasMaxLength(500);
        builder.Property(n => n.SenderName).HasMaxLength(100);
        builder.Property(n => n.RelatedEntityType).HasMaxLength(50);
        builder.HasIndex(n => new { n.RecipientId, n.Status });
        builder.HasIndex(n => n.CreatedAt);
    }
}

public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
    public void Configure(EntityTypeBuilder<NotificationPreference> builder)
    {
        builder.HasKey(np => np.Id);
        builder.HasIndex(np => new { np.UserId, np.NotificationType }).IsUnique();
    }
}

// ─── Commerce ───────────────────────────────────────────────────────────────

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.HasKey(w => w.Id);
        builder.HasIndex(w => new { w.UserId, w.CourseId }).IsUnique();
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);
        builder.HasIndex(ci => new { ci.UserId, ci.CourseId }).IsUnique();
        builder.Property(ci => ci.DiscountAmount).HasPrecision(18, 2);
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.TransactionId).HasMaxLength(100).IsRequired();
        builder.Property(p => p.PaymentMethod).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.Currency).HasMaxLength(10);
        builder.Property(p => p.RefundAmount).HasPrecision(18, 2);
        builder.Property(p => p.TaxAmount).HasPrecision(18, 2);
        builder.Property(p => p.PlatformFee).HasPrecision(18, 2);
        builder.Property(p => p.InstructorEarnings).HasPrecision(18, 2);
        builder.HasIndex(p => p.TransactionId).IsUnique();
        builder.HasIndex(p => p.Status);
        builder.HasMany(p => p.RefundRequests).WithOne(r => r.Payment).HasForeignKey(r => r.PaymentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class RefundRequestConfiguration : IEntityTypeConfiguration<RefundRequest>
{
    public void Configure(EntityTypeBuilder<RefundRequest> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Reason).HasMaxLength(2000).IsRequired();
        builder.Property(r => r.AdminNote).HasMaxLength(2000);
        builder.HasIndex(r => r.Status);
        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(r => r.ResolvedBy).WithMany().HasForeignKey(r => r.ResolvedById).OnDelete(DeleteBehavior.Restrict);
    }
}

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Code).HasMaxLength(50).IsRequired();
        builder.Property(c => c.Value).HasPrecision(18, 2);
        builder.Property(c => c.MaxDiscountAmount).HasPrecision(18, 2);
        builder.Property(c => c.MinOrderAmount).HasPrecision(18, 2);
        builder.HasIndex(c => c.Code).IsUnique();
        builder.HasMany(c => c.Usages).WithOne(cu => cu.Coupon).HasForeignKey(cu => cu.CouponId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.ApplicableCourses).WithOne(cc => cc.Coupon).HasForeignKey(cc => cc.CouponId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.ApplicableCategories).WithOne(cc => cc.Coupon).HasForeignKey(cc => cc.CouponId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
{
    public void Configure(EntityTypeBuilder<CouponUsage> builder)
    {
        builder.HasKey(cu => cu.Id);
        builder.Property(cu => cu.DiscountAmount).HasPrecision(18, 2);
        builder.HasOne(cu => cu.User).WithMany().HasForeignKey(cu => cu.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(cu => cu.Enrollment).WithMany().HasForeignKey(cu => cu.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PayoutConfiguration : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.TotalRevenue).HasPrecision(18, 2);
        builder.Property(p => p.PlatformFee).HasPrecision(18, 2);
        builder.HasIndex(p => p.Status);
        builder.HasOne(p => p.Instructor).WithMany().HasForeignKey(p => p.InstructorId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class BundleConfiguration : IEntityTypeConfiguration<Bundle>
{
    public void Configure(EntityTypeBuilder<Bundle> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).HasMaxLength(200).IsRequired();
        builder.Property(b => b.Price).HasPrecision(18, 2);
        builder.Property(b => b.DiscountPrice).HasPrecision(18, 2);
        builder.HasMany(b => b.BundleCourses).WithOne(bc => bc.Bundle).HasForeignKey(bc => bc.BundleId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class BundleCourseConfiguration : IEntityTypeConfiguration<BundleCourse>
{
    public void Configure(EntityTypeBuilder<BundleCourse> builder)
    {
        builder.HasKey(bc => bc.Id);
        builder.HasIndex(bc => new { bc.BundleId, bc.CourseId }).IsUnique();
    }
}

// ─── Analytics ──────────────────────────────────────────────────────────────

public class CourseAnalyticsConfiguration : IEntityTypeConfiguration<CourseAnalytics>
{
    public void Configure(EntityTypeBuilder<CourseAnalytics> builder)
    {
        builder.HasKey(ca => ca.Id);
        builder.HasIndex(ca => new { ca.CourseId, ca.Date }).IsUnique();
        builder.Property(ca => ca.Revenue).HasPrecision(18, 2);
        builder.Property(ca => ca.Refunds).HasPrecision(18, 2);
        builder.HasOne(ca => ca.Course).WithMany().HasForeignKey(ca => ca.CourseId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PlatformAnalyticsConfiguration : IEntityTypeConfiguration<PlatformAnalytics>
{
    public void Configure(EntityTypeBuilder<PlatformAnalytics> builder)
    {
        builder.HasKey(pa => pa.Id);
        builder.HasIndex(pa => pa.Date).IsUnique();
        builder.Property(pa => pa.TotalRevenue).HasPrecision(18, 2);
    }
}

public class SearchLogConfiguration : IEntityTypeConfiguration<SearchLog>
{
    public void Configure(EntityTypeBuilder<SearchLog> builder)
    {
        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Query).HasMaxLength(500).IsRequired();
        builder.HasIndex(sl => sl.SearchedAt);
        builder.HasOne(sl => sl.User).WithMany().HasForeignKey(sl => sl.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
