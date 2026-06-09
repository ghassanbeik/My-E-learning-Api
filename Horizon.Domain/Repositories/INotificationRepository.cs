

using Horizon.Domain.Entities;
using Horizon.Domain.Shared;

namespace Horizon.Domain.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByRecipientAsync(Guid recipientId, CancellationToken ct = default);
        Task<IEnumerable<Notification>> GetUnreadAsync(Guid recipientId, CancellationToken ct = default);
        Task<int> GetUnreadCountAsync(Guid recipientId, CancellationToken ct = default);
        Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default);
        Task MarkAllAsReadAsync(Guid recipientId, CancellationToken ct = default);
        Task<PagedResult<Notification>> GetPagedByRecipientAsync(Guid recipientId, int page, int pageSize, CancellationToken ct = default);
    }
}
