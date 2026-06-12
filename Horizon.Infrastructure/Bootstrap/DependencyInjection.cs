using Horizon.Application.EventHandlers.AuthEventHandlers;
using Horizon.Application.EventHandlers.CertificateEventHandlers;
using Horizon.Application.EventHandlers.CourseEventHandlers;
using Horizon.Application.EventHandlers.DiscussionEventHandlers;
using Horizon.Application.EventHandlers.EnrollmentEventHandlers;
using Horizon.Application.EventHandlers.LiveSessionEventHandlers;
using Horizon.Application.EventHandlers.PaymentEventHandlers;
using Horizon.Application.EventHandlers.PayoutEventHandlers;
using Horizon.Application.EventHandlers.ReviewEventHandlers;
using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.CertificateEvents;
using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.DiscussionEvents;
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.LiveSessionEvents;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Events.PayoutEvents;
using Horizon.Domain.Events.ReviewEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using Horizon.Domain.Interfaces.Services.CertificateServices;
using Horizon.Domain.Interfaces.Services.CurrentUserServices;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.JWTServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;
using Horizon.Domain.Interfaces.Services.PasswordHasher;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using Horizon.Domain.Interfaces.Services.SearchServices;
using Horizon.Domain.Interfaces.Services.StorageServices;
using Horizon.Domain.Interfaces.Services.VideoServices;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Horizon.Infrastructure.Repositories;
using Horizon.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Horizon.Infrastructure.Bootstrap
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ─── Database ────────────────────────────────────────────────────────
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql =>
                    {
                        sql.MigrationsAssembly("Horizon.Infrastructure");
                        sql.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null);
                        sql.CommandTimeout(60);
                    }));

            // ─── Unit of Work ────────────────────────────────────────────────────
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ─── Individual Repositories (optional — use UoW instead) ───────────
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IInstructorProfileRepository, InstructorProfileRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IInstructorSubscriberRepository, InstructorSubscriberRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonNoteRepository, LessonNoteRepository>();
            services.AddScoped<ILessonBookmarkRepository, LessonBookmarkRepository>();
            services.AddScoped<ILiveSessionRepository, LiveSessionRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IProgressRepository, ProgressRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentSubmissionRepository, AssignmentSubmissionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewResponseRepository, ReviewResponseRepository>();
            services.AddScoped<IDiscussionRepository, DiscussionRepository>();
            services.AddScoped<IDiscussionReplyRepository, DiscussionReplyRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationPreferenceRepository, NotificationPreferenceRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRefundRequestRepository, RefundRequestRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICouponCourseRepository, CouponCourseRepository>();
            services.AddScoped<ICouponCategoryRepository, CouponCategoryRepository>();
            services.AddScoped<ICouponUsageRepository, CouponUsageRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IPayoutRepository, PayoutRepository>();
            services.AddScoped<IBundleRepository, BundleRepository>();
            services.AddScoped<ICourseAnalyticsRepository, CourseAnalyticsRepository>();
            services.AddScoped<IPlatformAnalyticsRepository, PlatformAnalyticsRepository>();
            services.AddScoped<ISearchLogRepository, SearchLogRepository>();
            services.AddScoped<ICourseTagRepository, CourseTagRepository>();
            services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
            services.AddScoped<IReviewVoteRepository, ReviewVoteRepository>();

            // ─── Core Services ────────────────────────────────────────────────────
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<IVideoProcessingService, VideoProcessingService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // ─── Event Bus ────────────────────────────────────────────────────────
            services.AddScoped<IEventBus, InMemoryEventBus>();

            // ─── Auth Event Handlers ──────────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
            services.AddScoped<IDomainEventHandler<PasswordResetRequestedEvent>, PasswordResetRequestedEventHandler>();
            services.AddScoped<IDomainEventHandler<PasswordChangedEvent>, PasswordChangedEventHandler>();

            // ─── Course Event Handlers ────────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<CourseApprovedEvent>, CourseApprovedEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseRejectedEvent>, CourseRejectedEventHandler>();
            services.AddScoped<IDomainEventHandler<CoursePublishedEvent>, CoursePublishedEventHandler>();

            // ─── Enrollment Event Handlers ────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<StudentEnrolledEvent>, StudentEnrolledEventHandler>();
            services.AddScoped<IDomainEventHandler<CourseCompletedEvent>, CourseCompletedEventHandler>();
            services.AddScoped<IDomainEventHandler<LessonCompletedEvent>, LessonCompletedEventHandler>();

            // ─── Payment Event Handlers ───────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<PaymentCompletedEvent>, PaymentCompletedEventHandler>();
            services.AddScoped<IDomainEventHandler<RefundApprovedEvent>, RefundApprovedEventHandler>();

            // ─── Review Event Handlers ────────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<ReviewSubmittedEvent>, ReviewSubmittedEventHandler>();
            services.AddScoped<IDomainEventHandler<ReviewApprovedEvent>, ReviewApprovedEventHandler>();

            // ─── Certificate Event Handlers ───────────────────────────────────────
            services.AddScoped<IDomainEventHandler<CertificateIssuedEvent>, CertificateIssuedEventHandler>();

            // ─── Discussion Event Handlers ────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<DiscussionCreatedEvent>, DiscussionCreatedEventHandler>();
            services.AddScoped<IDomainEventHandler<DiscussionRepliedEvent>, DiscussionRepliedEventHandler>();

            // ─── Payout Event Handlers ────────────────────────────────────────────
            services.AddScoped<IDomainEventHandler<PayoutProcessedEvent>, PayoutProcessedEventHandler>();

            // ─── Live Session Event Handlers ──────────────────────────────────────
            services.AddScoped<IDomainEventHandler<LiveSessionStartingEvent>, LiveSessionStartingEventHandler>();

            // ─── HttpContext for CurrentUserService ───────────────────────────────
            services.AddHttpContextAccessor();

            return services;
        }
    }
}