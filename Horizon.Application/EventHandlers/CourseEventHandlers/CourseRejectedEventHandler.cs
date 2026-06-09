

using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.CourseEventHandlers
{
    public class CourseRejectedEventHandler : IDomainEventHandler<CourseRejectedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;

        public CourseRejectedEventHandler(IEmailService email, INotificationService notifications)
        {
            _email = email;
            _notifications = notifications;
        }

        public async Task HandleAsync(CourseRejectedEvent e, CancellationToken ct = default)
        {
            await _email.SendCourseRejectedAsync(e.InstructorEmail, e.InstructorName, e.CourseTitle, e.Reason, ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.InstructorId,
                Title = "Course review update",
                Message = $"'{e.CourseTitle}' requires changes before it can be published. Reason: {e.Reason}",
                Type = Domain.Enums.NotificationType.CourseRejected,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
            }, ct);
        }
    }
}
