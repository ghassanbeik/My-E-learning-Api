using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Horizon.Domain.Entities;
using System.Reflection;

namespace Horizon.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Identity & Auth
    public DbSet<UserInfo> Users => Set<UserInfo>();
    public DbSet<RoleInfo> Roles => Set<RoleInfo>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<InstructorProfile> InstructorProfiles => Set<InstructorProfile>();
    public DbSet<InstructorSubscriber> InstructorSubscribers => Set<InstructorSubscriber>();

    // Catalog
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseCategory> CourseCategories => Set<CourseCategory>();
    public DbSet<CourseTag> CourseTags => Set<CourseTag>();
    public DbSet<CourseInstructor> CourseInstructors => Set<CourseInstructor>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<LessonNote> LessonNotes => Set<LessonNote>();
    public DbSet<LessonBookmark> LessonBookmarks => Set<LessonBookmark>();
    public DbSet<LiveSession> LiveSessions => Set<LiveSession>();

    // Learning
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Progress> Progresses => Set<Progress>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<QuizAnswer> QuizAnswers => Set<QuizAnswer>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();

    // Engagement
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewResponse> ReviewResponses => Set<ReviewResponse>();
    public DbSet<ReviewVote> ReviewVotes => Set<ReviewVote>();
    public DbSet<Discussion> Discussions => Set<Discussion>();
    public DbSet<DiscussionReply> DiscussionReplies => Set<DiscussionReply>();
    public DbSet<DiscussionVote> DiscussionVotes => Set<DiscussionVote>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<AnnouncementRead> AnnouncementReads => Set<AnnouncementRead>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();

    // Commerce
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<RefundRequest> RefundRequests => Set<RefundRequest>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponCourse> CouponCourses => Set<CouponCourse>();
    public DbSet<CouponCategory> CouponCategories => Set<CouponCategory>();
    public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
    public DbSet<Payout> Payouts => Set<Payout>();
    public DbSet<Bundle> Bundles => Set<Bundle>();
    public DbSet<BundleCourse> BundleCourses => Set<BundleCourse>();

    // Analytics
    public DbSet<CourseAnalytics> CourseAnalytics => Set<CourseAnalytics>();
    public DbSet<PlatformAnalytics> PlatformAnalytics => Set<PlatformAnalytics>();
    public DbSet<SearchLog> SearchLogs => Set<SearchLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

    }
}
