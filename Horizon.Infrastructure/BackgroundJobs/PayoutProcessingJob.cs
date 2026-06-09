

using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PayoutEvents;
using Horizon.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.BackgroundJobs
{
    public class PayoutProcessingJob : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<PayoutProcessingJob> _logger;
        // Runs on the 1st of every month
        private static readonly TimeSpan Interval = TimeSpan.FromDays(1);

        public PayoutProcessingJob(IServiceProvider services, ILogger<PayoutProcessingJob> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.UtcNow.Day == 1)
                {
                    try
                    {
                        await ProcessPayoutsAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "PayoutProcessingJob failed.");
                    }
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task ProcessPayoutsAsync(CancellationToken ct)
        {
            using var scope = _services.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var pendingPayouts = await uow.Payouts.GetPendingAsync(ct);

            foreach (var payout in pendingPayouts)
            {
                payout.Status = Domain.Enums.PayoutStatus.Processing;
                await uow.Payouts.UpdateAsync(payout);
                await uow.SaveChangesAsync(ct);

                var instructor = await uow.Users.GetByIdAsync(payout.InstructorId, ct);
                if (instructor == null) continue;

                // In production — integrate with Stripe Connect or bank transfer API here
                payout.Status = Domain.Enums.PayoutStatus.Completed;
                payout.ProcessedAt = DateTime.UtcNow;
                await uow.Payouts.UpdateAsync(payout);
                await uow.SaveChangesAsync(ct);

                await eventBus.PublishAsync(new PayoutProcessedEvent
                {
                    PayoutId = payout.Id,
                    InstructorId = payout.InstructorId,
                    InstructorEmail = instructor.Email,
                    InstructorName = instructor.FullName,
                    Amount = payout.Amount,
                    PeriodStart = payout.PeriodStart.ToString("MMM dd, yyyy"),
                    PeriodEnd = payout.PeriodEnd.ToString("MMM dd, yyyy"),
                }, ct);

                _logger.LogInformation("Processed payout {PayoutId} for instructor {InstructorId}.",
                    payout.Id, payout.InstructorId);
            }
        }
    }
}
