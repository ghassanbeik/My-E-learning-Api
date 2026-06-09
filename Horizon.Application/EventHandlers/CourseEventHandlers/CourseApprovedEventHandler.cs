

using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.CourseEventHandlers
{
    public class CourseApprovedEventHandler : IDomainEventHandler<CourseApprovedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;

        public CourseApprovedEventHandler(IEmailService email, INotificationService notifications)
        {
            _email = email;
            _notifications = notifications;
        }

        public async Task HandleAsync(CourseApprovedEvent e, CancellationToken ct = default)
        {
            await _email.SendCourseApprovedAsync(e.InstructorEmail, e.InstructorName, e.CourseTitle, ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.InstructorId,
                Title = "Your course has been approved!",
                Message = $"Congratulations! '{e.CourseTitle}' is now live on the platform.",
                Type = Domain.Enums.NotificationType.CourseApproved,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
            }, ct);
        }
    }

   

   
}
