

using Horizon.Domain.Events.CertificateEvents;
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.CertificateServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.EnrollmentEventHandlers
{
    public class CourseCompletedEventHandler : IDomainEventHandler<CourseCompletedEvent>
    {
        private readonly ICertificateService _certificates;
        private readonly INotificationService _notifications;
        private readonly IEventBus _eventBus;

        public CourseCompletedEventHandler(
            ICertificateService certificates,
            INotificationService notifications,
            IEventBus eventBus)
        {
            _certificates = certificates;
            _notifications = notifications;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(CourseCompletedEvent e, CancellationToken ct = default)
        {
            // Auto-generate certificate
            var certificate = await _certificates.GenerateAsync(e.EnrollmentId, ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.StudentId,
                Title = "Course completed! 🎉",
                Message = $"Congratulations on completing '{e.CourseTitle}'! Your certificate is ready.",
                Type = Domain.Enums.NotificationType.CourseCompleted,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
            }, ct);

            // Publish certificate issued event
            await _eventBus.PublishAsync(new CertificateIssuedEvent
            {
                CertificateId = certificate.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                StudentEmail = e.StudentEmail,
                StudentName = e.StudentName,
                CourseTitle = e.CourseTitle,
                CertificateNumber = certificate.CertificateNumber,
                VerificationUrl = certificate.VerificationUrl,
            }, ct);
        }
    }
}
