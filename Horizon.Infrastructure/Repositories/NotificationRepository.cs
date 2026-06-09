
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Domain.Shared;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Notification>> GetByRecipientAsync(Guid recipientId, CancellationToken ct = default)
            => await _dbSet
                .Where(n => n.RecipientId == recipientId)
                .OrderByDescending(n => n.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Notification>> GetUnreadAsync(Guid recipientId, CancellationToken ct = default)
            => await _dbSet
                .Where(n => n.RecipientId == recipientId && n.Status == NotificationStatus.Unread)
                .OrderByDescending(n => n.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<int> GetUnreadCountAsync(Guid recipientId, CancellationToken ct = default)
            => await _dbSet.CountAsync(n => n.RecipientId == recipientId && n.Status == NotificationStatus.Unread, ct);

        public async Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default)
        {
            var notification = await _dbSet.FindAsync(new object[] { notificationId }, ct);
            if (notification == null) return;
            notification.Status = NotificationStatus.Read;
            notification.ReadAt = DateTime.UtcNow;
        }

        public async Task MarkAllAsReadAsync(Guid recipientId, CancellationToken ct = default)
        {
            var notifications = await _dbSet
                .Where(n => n.RecipientId == recipientId && n.Status == NotificationStatus.Unread)
                .ToListAsync(ct);
            foreach (var n in notifications)
            {
                n.Status = NotificationStatus.Read;
                n.ReadAt = DateTime.UtcNow;
            }
        }

        public async Task<PagedResult<Notification>> GetPagedByRecipientAsync(Guid recipientId, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbSet.Where(n => n.RecipientId == recipientId);
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PagedResult<Notification> { Items = items, TotalCount = total, PageNumber = page, PageSize = pageSize };
        }
    }

}
