

using Horizon.Domain.Events.DiscussionEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.DiscussionEventHandlers
{
    public class DiscussionCreatedEventHandler : IDomainEventHandler<DiscussionCreatedEvent>
    {
        private readonly INotificationService _notifications;

        public DiscussionCreatedEventHandler(INotificationService notifications)
            => _notifications = notifications;

        public async Task HandleAsync(DiscussionCreatedEvent e, CancellationToken ct = default)
        {
            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.InstructorId,
                Title = "New question in your course",
                Message = $"{e.StudentName} asked: \"{e.DiscussionTitle}\" in '{e.CourseTitle}'.",
                Type = Domain.Enums.NotificationType.NewDiscussion,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.DiscussionId,
                RelatedEntityType = "Discussion",
                SenderName = e.StudentName,
            }, ct);
        }
    }

   

}
