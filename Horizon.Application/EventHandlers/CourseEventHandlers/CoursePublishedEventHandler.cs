
using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.CourseEventHandlers
{
    public class CoursePublishedEventHandler : IDomainEventHandler<CoursePublishedEvent>
    {
        private readonly INotificationService _notifications;
        private readonly IUnitOfWork _uow;

        public CoursePublishedEventHandler(INotificationService notifications, IUnitOfWork uow)
        {
            _notifications = notifications;
            _uow = uow;
        }

        public async Task HandleAsync(CoursePublishedEvent e, CancellationToken ct = default)
        {
            // Notify all instructor's subscribers
            var subscribers = await _uow.InstructorSubscribers.GetSubscribersAsync(e.InstructorId, ct);

            var recipientIds = subscribers.Select(s => s.Id).ToList();
            if (!recipientIds.Any()) return;

            await _notifications.SendToManyAsync(recipientIds, new SendNotificationRequest
            {
                RecipientId = Guid.Empty, // overridden per recipient
                Title = $"New course from {e.InstructorName}",
                Message = $"'{e.CourseTitle}' is now available. Check it out!",
                Type = Domain.Enums.NotificationType.NewContent,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
                SenderName = e.InstructorName,
                SenderId = e.InstructorId,
            }, ct);
        }
    }
}
