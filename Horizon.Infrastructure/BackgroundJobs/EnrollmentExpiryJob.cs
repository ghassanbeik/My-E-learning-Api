

using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Horizon.Infrastructure.BackgroundJobs
{
    public class EnrollmentExpiryJob : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<EnrollmentExpiryJob> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromHours(6);

        public EnrollmentExpiryJob(IServiceProvider services, ILogger<EnrollmentExpiryJob> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EnrollmentExpiryJob started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "EnrollmentExpiryJob failed.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task ProcessAsync(CancellationToken ct)
        {
            using var scope = _services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var expiring = await uow.Enrollments.GetExpiringAsync(0, ct); // 0 = already expired

            foreach (var enrollment in expiring)
            {
                enrollment.Status = Domain.Enums.EnrollmentStatus.Expired;
                await uow.Enrollments.UpdateAsync(enrollment);

                var student = await uow.Users.GetByIdAsync(enrollment.StudentId, ct);
                var course = await uow.Courses.GetByIdAsync(enrollment.CourseId, ct);

                if (student != null && course != null)
                {
                    await eventBus.PublishAsync(new EnrollmentExpiredEvent
                    {
                        EnrollmentId = enrollment.Id,
                        StudentId = enrollment.StudentId,
                        CourseId = enrollment.CourseId,
                        StudentEmail = student.Email,
                        StudentName = student.FullName,
                        CourseTitle = course.Title,
                    }, ct);
                }
            }

            if (expiring.Any())
            {
                await uow.SaveChangesAsync(ct);
                _logger.LogInformation("Expired {Count} enrollments.", expiring.Count());
            }
        }
    }

}
