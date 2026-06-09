

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class NotificationPreference : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public NotificationType NotificationType { get; set; } = NotificationType.SystemAnnouncement;
        public bool InAppEnabled { get; set; } = true;
        public bool EmailEnabled { get; set; } = true;
        public bool PushEnabled { get; set; } = false;
        public bool SmsEnabled { get; set; } = false;
        public string? EmailTemplate { get; set; }
        public DateTime? MutedUntil { get; set; }
    }
}
