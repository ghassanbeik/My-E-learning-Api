

using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.LiveSessionEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Horizon.Infrastructure.BackgroundJobs
{
    public class LiveSessionReminderJob : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<LiveSessionReminderJob> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(15);

        public LiveSessionReminderJob(IServiceProvider services, ILogger<LiveSessionReminderJob> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LiveSessionReminderJob started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LiveSessionReminderJob failed.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task ProcessAsync(CancellationToken ct)
        {
            using var scope = _services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            // Find sessions starting in the next 15-30 minutes
            var windowStart = DateTime.UtcNow.AddMinutes(15);
            var windowEnd = DateTime.UtcNow.AddMinutes(30);

            var allCourses = await uow.Courses.GetAllAsync(ct);

            foreach (var course in allCourses)
            {
                var sessions = await uow.LiveSessions.GetUpcomingAsync(course.Id, ct);
                var upcoming = sessions.Where(s =>
                    s.ScheduledAt >= windowStart &&
                    s.ScheduledAt <= windowEnd &&
                    !s.IsCompleted).ToList();

                foreach (var session in upcoming)
                {
                    await eventBus.PublishAsync(new LiveSessionStartingEvent
                    {
                        SessionId = session.Id,
                        CourseId = course.Id,
                        SessionTitle = session.Title,
                        MeetingUrl = session.MeetingUrl ?? string.Empty,
                        ScheduledAt = session.ScheduledAt,
                    }, ct);

                    _logger.LogInformation("Sent reminder for live session {SessionId}.", session.Id);
                }
            }
        }
    }
}
