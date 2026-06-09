
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.ReviewEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.ReviewEventHandlers
{
    public class ReviewSubmittedEventHandler : IDomainEventHandler<ReviewSubmittedEvent>
    {
        private readonly INotificationService _notifications;
        private readonly ICacheService _cache;

        public ReviewSubmittedEventHandler(INotificationService notifications, ICacheService cache)
        {
            _notifications = notifications;
            _cache = cache;
        }

        public async Task HandleAsync(ReviewSubmittedEvent e, CancellationToken ct = default)
        {
            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.InstructorId,
                Title = "New review received",
                Message = $"{e.StudentName} left a {e.Rating}-star review on '{e.CourseTitle}'.",
                Type = Domain.Enums.NotificationType.NewReview,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.CourseId,
                RelatedEntityType = "Course",
                SenderName = e.StudentName,
                SenderId = e.StudentId,
            }, ct);
        }
    }
}
