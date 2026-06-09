

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;

namespace Horizon.Domain.Repositories
{
    public interface INotificationPreferenceRepository : IRepository<NotificationPreference>
    {
        Task<IEnumerable<NotificationPreference>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<NotificationPreference?> GetByUserAndTypeAsync(Guid userId, NotificationType type, CancellationToken ct = default);
        Task UpsertAsync(NotificationPreference preference, CancellationToken ct = default);
    }
}
