

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class NotificationPreferenceRepository : Repository<NotificationPreference>, INotificationPreferenceRepository
    {
        public NotificationPreferenceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<NotificationPreference>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet.Where(np => np.UserId == userId).AsNoTracking().ToListAsync(ct);

        public async Task<NotificationPreference?> GetByUserAndTypeAsync(Guid userId, NotificationType type, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(np => np.UserId == userId && np.NotificationType == type, ct);

        public async Task UpsertAsync(NotificationPreference preference, CancellationToken ct = default)
        {
            var existing = await GetByUserAndTypeAsync(preference.UserId, preference.NotificationType, ct);
            if (existing == null)
                await _dbSet.AddAsync(preference, ct);
            else
            {
                existing.InAppEnabled = preference.InAppEnabled;
                existing.EmailEnabled = preference.EmailEnabled;
                existing.PushEnabled = preference.PushEnabled;
                existing.SmsEnabled = preference.SmsEnabled;
                existing.MutedUntil = preference.MutedUntil;
            }
        }
    }
}
