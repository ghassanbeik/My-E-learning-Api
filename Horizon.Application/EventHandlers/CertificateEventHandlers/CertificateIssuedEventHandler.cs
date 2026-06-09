

using Horizon.Domain.Events.CertificateEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.CertificateEventHandlers
{
    public class CertificateIssuedEventHandler : IDomainEventHandler<CertificateIssuedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;

        public CertificateIssuedEventHandler(IEmailService email, INotificationService notifications)
        {
            _email = email;
            _notifications = notifications;
        }

        public async Task HandleAsync(CertificateIssuedEvent e, CancellationToken ct = default)
        {
            await _email.SendCertificateAsync(
                e.StudentEmail, e.StudentName, e.CourseTitle,
                e.VerificationUrl ?? string.Empty, ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.StudentId,
                Title = "Certificate issued!",
                Message = $"Your certificate for '{e.CourseTitle}' is ready. Certificate #: {e.CertificateNumber}",
                Type = Domain.Enums.NotificationType.CertificateIssued,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CertificateId,
                RelatedEntityType = "Certificate",
            }, ct);
        }
    }

}
