
namespace Horizon.Application.DTOs
{
    public record NotificationDto(
       Guid Id,
       string Title,
       string Message,
       string Type,
       string Channel,
       string Status,
       bool IsUnread,
       string? ActionUrl,
       string? ImageUrl,
       Guid? RelatedEntityId,
       string? RelatedEntityType,
       DateTime? ReadAt,
       DateTime CreatedAt);

    public record NotificationPreferenceDto(
        Guid Id,
        string NotificationType,
        bool InAppEnabled,
        bool EmailEnabled,
        bool PushEnabled,
        bool SmsEnabled);

    public record UpdateNotificationPreferenceDto(
        string NotificationType,
        bool InAppEnabled,
        bool EmailEnabled,
        bool PushEnabled,
        bool SmsEnabled);
}
