

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public Guid RecipientId { get; set; }
        public UserInfo Recipient { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; } = NotificationType.SystemAnnouncement;
        public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
        public string? ActionUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? Metadata { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public int RetryCount { get; set; } = 0;
        public string? ErrorMessage { get; set; }
        public string? SenderName { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string? RelatedEntityType { get; set; }
    }
}
