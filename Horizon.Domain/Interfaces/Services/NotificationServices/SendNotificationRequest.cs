using Horizon.Domain.Enums;

namespace Horizon.Domain.Interfaces.Services.NotificationServices
{
    public class SendNotificationRequest
    {
        public Guid RecipientId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
        public string? ActionUrl { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string? RelatedEntityType { get; set; }
        public string? SenderName { get; set; }
        public Guid? SenderId { get; set; }
    }
}
