

using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PayoutEvents;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.PayoutEventHandlers
{
    public class PayoutProcessedEventHandler : IDomainEventHandler<PayoutProcessedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;

        public PayoutProcessedEventHandler(IEmailService email, INotificationService notifications)
        {
            _email = email;
            _notifications = notifications;
        }

        public async Task HandleAsync(PayoutProcessedEvent e, CancellationToken ct = default)
        {
            await _email.SendPayoutNotificationAsync(
                e.InstructorEmail, e.InstructorName, e.Amount,
                $"{e.PeriodStart} - {e.PeriodEnd}", ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.InstructorId,
                Title = "Payout processed",
                Message = $"Your payout of ${e.Amount:F2} for the period {e.PeriodStart} - {e.PeriodEnd} has been processed.",
                Type = Domain.Enums.NotificationType.PayoutProcessed,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.PayoutId,
                RelatedEntityType = "Payout",
            }, ct);
        }
    }

}
