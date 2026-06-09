
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.PaymentEventHandlers
{
    public class RefundApprovedEventHandler : IDomainEventHandler<RefundApprovedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;

        public RefundApprovedEventHandler(IEmailService email, INotificationService notifications)
        {
            _email = email;
            _notifications = notifications;
        }

        public async Task HandleAsync(RefundApprovedEvent e, CancellationToken ct = default)
        {
            await _email.SendRefundConfirmationAsync(e.UserEmail, e.UserName, e.CourseTitle, e.Amount, ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.UserId,
                Title = "Refund approved",
                Message = $"Your refund of ${e.Amount:F2} for '{e.CourseTitle}' has been approved.",
                Type = Domain.Enums.NotificationType.PaymentReceived,
                Channel = Domain.Enums.NotificationChannel.InApp,
            }, ct);
        }
    }
}
