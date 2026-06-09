namespace Horizon.Domain.Interfaces.Services.NotificationServices
{
    public interface INotificationService
    {
        Task SendAsync(SendNotificationRequest request, CancellationToken ct = default);
        Task SendToManyAsync(IEnumerable<Guid> recipientIds, SendNotificationRequest request, CancellationToken ct = default);
        Task SendToRoleAsync(string role, SendNotificationRequest request, CancellationToken ct = default);
        Task MarkReadAsync(Guid notificationId, CancellationToken ct = default);
        Task MarkAllReadAsync(Guid userId, CancellationToken ct = default);
        Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
    }
}
