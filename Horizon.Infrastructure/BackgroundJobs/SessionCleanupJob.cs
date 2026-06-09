

using Horizon.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.BackgroundJobs
{
    public class SessionCleanupJob : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<SessionCleanupJob> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromHours(12);

        public SessionCleanupJob(IServiceProvider services, ILogger<SessionCleanupJob> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SessionCleanupJob failed.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task CleanupAsync(CancellationToken ct)
        {
            using var scope = _services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var expiredSessions = await uow.Sessions.FindAsync(
                s => s.ExpiresAt < DateTime.UtcNow && s.RevokedAt == null, ct);

            foreach (var session in expiredSessions)
            {
                session.RevokedAt = DateTime.UtcNow;
                await uow.Sessions.UpdateAsync(session);
            }

            if (expiredSessions.Any())
            {
                await uow.SaveChangesAsync(ct);
                _logger.LogInformation("Cleaned up {Count} expired sessions.", expiredSessions.Count());
            }
        }
    }

}
