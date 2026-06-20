using Horizon.Domain.Interfaces;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;


namespace Horizon.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    // ─── Identity & Auth ─────────────────────────────────────────────────────
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IPermissionRepository? _permissions;
    private IUserRoleRepository? _userRoles;
    private IInstructorProfileRepository? _instructorProfiles;
    private ISessionRepository? _sessions;
    private IInstructorSubscriberRepository? _instructorSubscribers;

    // ─── Catalog ─────────────────────────────────────────────────────────────
    private ICourseRepository? _courses;
    private ICourseCategoryRepository? _courseCategories;
    private ICourseTagRepository? _courseTags;
    private ICategoryRepository? _categories;
    private ITagRepository? _tags;
    private ISectionRepository? _sections;
    private ILessonRepository? _lessons;
    private ILessonNoteRepository? _lessonNotes;
    private ILessonBookmarkRepository? _lessonBookmarks;
    private ILiveSessionRepository? _liveSessions;

    // ─── Learning ────────────────────────────────────────────────────────────
    private IEnrollmentRepository? _enrollments;
    private IProgressRepository? _progresses;
    private ICertificateRepository? _certificates;
    private IQuizRepository? _quizzes;
    private IQuizAttemptRepository? _quizAttempts;
    private IQuestionRepository? _questions;
    private IAnswerOptionRepository? _answerOptions;
    private IAssignmentRepository? _assignments;
    private IAssignmentSubmissionRepository? _assignmentSubmissions;

    // ─── Engagement ──────────────────────────────────────────────────────────
    private IReviewRepository? _reviews;
    private IReviewVoteRepository? _reviewVotes;
    private IDiscussionRepository? _discussions;
    private IDiscussionVoteRepository? _discussionVotes;
    private IDiscussionReplyRepository? _discussionReplies;
    private IAnnouncementRepository? _announcements;
    private INotificationRepository? _notifications;
    private INotificationPreferenceRepository? _notificationPreferences;

    // ─── Commerce ────────────────────────────────────────────────────────────
    private IPaymentRepository? _payments;
    private IRefundRequestRepository? _refundRequests;
    private ICouponRepository? _coupons;
    private ICouponCourseRepository? _couponCourses;
    private ICouponCategoryRepository? _couponCategories;
    private ICouponUsageRepository? _couponUsages;
    private IWishlistRepository? _wishlists;
    private ICartItemRepository? _cartItems;
    private IPayoutRepository? _payouts;
    private IBundleRepository? _bundles;

    // ─── Analytics ───────────────────────────────────────────────────────────
    private ICourseAnalyticsRepository? _courseAnalytics;
    private IPlatformAnalyticsRepository? _platformAnalytics;
    private ISearchLogRepository? _searchLogs;
    private IVerificationTokenRepository? _verificationTokens;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // ─── Identity & Auth Properties ──────────────────────────────────────────
    public IUserRepository Users
        => _users ??= new UserRepository(_context);
    public IRoleRepository Roles
        => _roles ??= new RoleRepository(_context);
    public IPermissionRepository Permissions
        => _permissions ??= new PermissionRepository(_context);
    public IUserRoleRepository UserRoles
        => _userRoles ??= new UserRoleRepository(_context);
    public IInstructorProfileRepository InstructorProfiles
        => _instructorProfiles ??= new InstructorProfileRepository(_context);
    public ISessionRepository Sessions
        => _sessions ??= new SessionRepository(_context);
    public IInstructorSubscriberRepository InstructorSubscribers
        => _instructorSubscribers ??= new InstructorSubscriberRepository(_context);

    // ─── Catalog Properties ──────────────────────────────────────────────────
    public ICourseRepository Courses
        => _courses ??= new CourseRepository(_context);
    public ICourseCategoryRepository CourseCategories
        => _courseCategories ??= new CourseCategoryRepository(_context);

    public ICourseTagRepository CourseTags
       => _courseTags ??= new CourseTagRepository(_context);
    public ICategoryRepository Categories
        => _categories ??= new CategoryRepository(_context);
    public ITagRepository Tags
        => _tags ??= new TagRepository(_context);
    public ISectionRepository Sections
        => _sections ??= new SectionRepository(_context);
    public ILessonRepository Lessons
        => _lessons ??= new LessonRepository(_context);
    public ILessonNoteRepository LessonNotes
        => _lessonNotes ??= new LessonNoteRepository(_context);
    public ILessonBookmarkRepository LessonBookmarks
        => _lessonBookmarks ??= new LessonBookmarkRepository(_context);
    public ILiveSessionRepository LiveSessions
        => _liveSessions ??= new LiveSessionRepository(_context);

    // ─── Learning Properties ─────────────────────────────────────────────────
    public IEnrollmentRepository Enrollments
        => _enrollments ??= new EnrollmentRepository(_context);
    public IProgressRepository Progresses
        => _progresses ??= new ProgressRepository(_context);
    public ICertificateRepository Certificates
        => _certificates ??= new CertificateRepository(_context);
    public IQuizRepository Quizzes
        => _quizzes ??= new QuizRepository(_context);
    public IQuizAttemptRepository QuizAttempts
        => _quizAttempts ??= new QuizAttemptRepository(_context);
    public IAssignmentRepository Assignments
        => _assignments ??= new AssignmentRepository(_context);
    public IAssignmentSubmissionRepository AssignmentSubmissions
        => _assignmentSubmissions ??= new AssignmentSubmissionRepository(_context);

    // ─── Engagement Properties ────────────────────────────────────────────────
    public IReviewRepository Reviews
        => _reviews ??= new ReviewRepository(_context);
    public IReviewVoteRepository ReviewVotes
        => _reviewVotes ??= new ReviewVoteRepository(_context);
    public IDiscussionRepository Discussions
        => _discussions ??= new DiscussionRepository(_context);
    public IDiscussionVoteRepository DiscussionVotes
        => _discussionVotes ??= new DiscussionVoteRepository(_context);
    public IDiscussionReplyRepository DiscussionReplies
        => _discussionReplies ??= new DiscussionReplyRepository(_context);
    public IAnnouncementRepository Announcements
        => _announcements ??= new AnnouncementRepository(_context);
    public INotificationRepository Notifications
        => _notifications ??= new NotificationRepository(_context);
    public INotificationPreferenceRepository NotificationPreferences
        => _notificationPreferences ??= new NotificationPreferenceRepository(_context);

    // ─── Commerce Properties ──────────────────────────────────────────────────
    public IPaymentRepository Payments
        => _payments ??= new PaymentRepository(_context);
    public IRefundRequestRepository RefundRequests
        => _refundRequests ??= new RefundRequestRepository(_context);
    public ICouponRepository Coupons
        => _coupons ??= new CouponRepository(_context);
    public ICouponCourseRepository CouponCourses
        => _couponCourses ??= new CouponCourseRepository(_context);
    public ICouponCategoryRepository CouponCategories
        => _couponCategories ??= new CouponCategoryRepository(_context);
    public ICouponUsageRepository CouponUsages
        => _couponUsages ??= new CouponUsageRepository(_context);
    public IWishlistRepository Wishlists
        => _wishlists ??= new WishlistRepository(_context);
    public ICartItemRepository CartItems
        => _cartItems ??= new CartItemRepository(_context);
    public IPayoutRepository Payouts
        => _payouts ??= new PayoutRepository(_context);
    public IBundleRepository Bundles
        => _bundles ??= new BundleRepository(_context);

    // ─── Analytics Properties ────────────────────────────────────────────────
    public ICourseAnalyticsRepository CourseAnalytics
        => _courseAnalytics ??= new CourseAnalyticsRepository(_context);
    public IPlatformAnalyticsRepository PlatformAnalytics
        => _platformAnalytics ??= new PlatformAnalyticsRepository(_context);
    public ISearchLogRepository SearchLogs
        => _searchLogs ??= new SearchLogRepository(_context);

    public IVerificationTokenRepository VerificationTokens
        => _verificationTokens ??= new VerificationTokenRepository(_context);

    public IQuestionRepository Questions 
        => _questions??=new QuestionRepository(_context);

    public IAnswerOptionRepository AnswerOptions 
        => _answerOptions ??= new AnswerOptionRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
