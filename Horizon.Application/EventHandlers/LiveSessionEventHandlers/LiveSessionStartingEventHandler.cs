using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.LiveSessionEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;


namespace Horizon.Application.EventHandlers.LiveSessionEventHandlers
{
    public class LiveSessionStartingEventHandler : IDomainEventHandler<LiveSessionStartingEvent>
    {
        private readonly INotificationService _notifications;
        private readonly IEmailService _email;
        private readonly IUnitOfWork _uow;

        public LiveSessionStartingEventHandler(
            INotificationService notifications,
            IEmailService email,
            IUnitOfWork uow)
        {
            _notifications = notifications;
            _email = email;
            _uow = uow;
        }

        public async Task HandleAsync(LiveSessionStartingEvent e, CancellationToken ct = default)
        {
            // Notify all enrolled active students
            var enrollments = await _uow.Enrollments.GetByCourseAsync(e.CourseId, ct);
            var studentIds = enrollments
                .Where(en => en.Status == Domain.Enums.EnrollmentStatus.Active)
                .Select(en => en.StudentId)
                .ToList();

            await _notifications.SendToManyAsync(studentIds, new SendNotificationRequest
            {
                RecipientId = Guid.Empty,
                Title = "Live session starting soon!",
                Message = $"'{e.SessionTitle}' starts in 15 minutes. Join now!",
                Type = Domain.Enums.NotificationType.LiveSessionStarting,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.SessionId,
                RelatedEntityType = "LiveSession",
            }, ct);
        }
    }
}
