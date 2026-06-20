using Horizon.Domain.Repositories;

namespace Horizon.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // ─── Identity & Auth ─────────────────────────────────────────────────────
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    IUserRoleRepository UserRoles { get; }
    IInstructorProfileRepository InstructorProfiles { get; }
    ISessionRepository Sessions { get; }
    IInstructorSubscriberRepository InstructorSubscribers { get; }

    // ─── Catalog ─────────────────────────────────────────────────────────────
    ICourseRepository Courses { get; }

    ICourseCategoryRepository CourseCategories { get; }
    ICourseTagRepository CourseTags { get; }
    ICategoryRepository Categories { get; }
    ITagRepository Tags { get; }
    ISectionRepository Sections { get; }
    ILessonRepository Lessons { get; }
    ILessonNoteRepository LessonNotes { get; }
    ILessonBookmarkRepository LessonBookmarks { get; }
    ILiveSessionRepository LiveSessions { get; }

    // ─── Learning ────────────────────────────────────────────────────────────
    IEnrollmentRepository Enrollments { get; }
    IProgressRepository Progresses { get; }
    ICertificateRepository Certificates { get; }
    IQuizRepository Quizzes { get; }
    IQuizAttemptRepository QuizAttempts { get; }
    IQuestionRepository Questions { get; }
    IAnswerOptionRepository AnswerOptions { get; }
    IAssignmentRepository Assignments { get; }
    IAssignmentSubmissionRepository AssignmentSubmissions { get; }

    // ─── Engagement ──────────────────────────────────────────────────────────
    IReviewRepository Reviews { get; }
    IReviewVoteRepository ReviewVotes { get; }
    IDiscussionRepository Discussions { get; }
    IDiscussionVoteRepository DiscussionVotes { get; }
    IDiscussionReplyRepository DiscussionReplies { get; }
    IAnnouncementRepository Announcements { get; }
    INotificationRepository Notifications { get; }
    INotificationPreferenceRepository NotificationPreferences { get; }

    // ─── Commerce ────────────────────────────────────────────────────────────
    IPaymentRepository Payments { get; }
    IRefundRequestRepository RefundRequests { get; }
    ICouponRepository Coupons { get; }
    ICouponCourseRepository CouponCourses { get; }
    ICouponCategoryRepository CouponCategories { get; }
    ICouponUsageRepository CouponUsages { get; }
    IWishlistRepository Wishlists { get; }
    ICartItemRepository CartItems { get; }
    IPayoutRepository Payouts { get; }
    IBundleRepository Bundles { get; }

    // ─── Analytics ───────────────────────────────────────────────────────────
    ICourseAnalyticsRepository CourseAnalytics { get; }
    IPlatformAnalyticsRepository PlatformAnalytics { get; }
    ISearchLogRepository SearchLogs { get; }
    IVerificationTokenRepository VerificationTokens { get; }

    // ─── Transactions ────────────────────────────────────────────────────────
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}