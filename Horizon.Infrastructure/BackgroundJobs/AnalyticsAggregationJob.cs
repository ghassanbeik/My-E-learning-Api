

using Horizon.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.BackgroundJobs
{
    public class AnalyticsAggregationJob : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AnalyticsAggregationJob> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

        public AnalyticsAggregationJob(IServiceProvider services, ILogger<AnalyticsAggregationJob> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wait until midnight UTC
            var now = DateTime.UtcNow;
            var nextRun = now.Date.AddDays(1);
            var initialDelay = nextRun - now;

            _logger.LogInformation("AnalyticsAggregationJob will start at {NextRun}.", nextRun);
            await Task.Delay(initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await AggregateAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "AnalyticsAggregationJob failed.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task AggregateAsync(CancellationToken ct)
        {
            using var scope = _services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var yesterday = DateTime.UtcNow.Date.AddDays(-1);
            _logger.LogInformation("Aggregating analytics for {Date}.", yesterday);

            // Aggregate course-level stats
            var courses = await uow.Courses.FindAsync(c => c.Status == Domain.Enums.CourseStatus.Published, ct);

            foreach (var course in courses)
            {
                var enrollments = await uow.Enrollments.GetByCourseAsync(course.Id, ct);
                var todayEnrollments = enrollments
                    .Where(e => e.EnrolledAt.Date == yesterday).ToList();

                var completions = enrollments
                    .Where(e => e.CompletedAt?.Date == yesterday).ToList();

                var payments = await uow.Payments.GetByEnrollmentAsync(
                    enrollments.FirstOrDefault()?.Id ?? Guid.Empty, ct);

                var revenue = todayEnrollments.Sum(e => e.AmountPaid);

                var analytics = new Domain.Entities.CourseAnalytics
                {
                    CourseId = course.Id,
                    Date = yesterday,
                    NewEnrollments = todayEnrollments.Count,
                    Completions = completions.Count,
                    Revenue = revenue,
                    AverageRating = course.AverageRating,
                    AverageProgress = enrollments.Any()
                        ? (double)enrollments.Average(e => e.ProgressPercentage)
                        : 0,
                };

                await uow.CourseAnalytics.UpsertDailyAsync(analytics, ct);
            }

            // Aggregate platform-level stats
            var newUsers = await uow.Users.CountAsync(
                u => u.CreatedAt.Date == yesterday, ct);

            var newCourses = await uow.Courses.CountAsync(
                c => c.CreatedAt.Date == yesterday, ct);

            var totalEnrollments = await uow.Enrollments.CountAsync(
                e => e.EnrolledAt.Date == yesterday, ct);

            var totalRevenue = await uow.Payments.GetTotalRevenueAsync(
                yesterday, yesterday.AddDays(1), ct);

            var certs = await uow.Certificates.CountAsync(
                c => c.IssueDate.Date == yesterday, ct);

            var platformAnalytics = new Domain.Entities.PlatformAnalytics
            {
                Date = yesterday,
                NewUsers = newUsers,
                NewCourses = newCourses,
                TotalEnrollments = totalEnrollments,
                TotalRevenue = totalRevenue,
                CertificatesIssued = certs,
            };

            await uow.PlatformAnalytics.UpsertDailyAsync(platformAnalytics, ct);
            await uow.SaveChangesAsync(ct);

            _logger.LogInformation("Analytics aggregation complete for {Date}.", yesterday);
        }
    }
}
