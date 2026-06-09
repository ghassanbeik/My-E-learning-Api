

using Horizon.Domain.Events.DiscussionEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.DiscussionEventHandlers
{
    public class DiscussionRepliedEventHandler : IDomainEventHandler<DiscussionRepliedEvent>
    {
        private readonly INotificationService _notifications;

        public DiscussionRepliedEventHandler(INotificationService notifications)
            => _notifications = notifications;

        public async Task HandleAsync(DiscussionRepliedEvent e, CancellationToken ct = default)
        {
            if (e.DiscussionAuthorId == e.ReplierId) return;

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.DiscussionAuthorId,
                Title = e.IsInstructorReply ? "Instructor replied to your question" : "New reply to your question",
                Message = $"{e.ReplierName} replied to your discussion.",
                Type = Domain.Enums.NotificationType.DiscussionReply,
                Channel = Domain.Enums.NotificationChannel.InApp,
                RelatedEntityId = e.DiscussionId,
                RelatedEntityType = "Discussion",
                SenderName = e.ReplierName,
                SenderId = e.ReplierId,
            }, ct);
        }
    }
}
