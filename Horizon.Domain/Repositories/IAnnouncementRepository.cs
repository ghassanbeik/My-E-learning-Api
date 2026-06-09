
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        Task<IEnumerable<Announcement>> GetByCourseAsync(Guid courseId, CancellationToken ct = default);
        Task<IEnumerable<Announcement>> GetUnreadByUserAsync(Guid userId, CancellationToken ct = default);
        Task MarkAsReadAsync(Guid announcementId, Guid userId, CancellationToken ct = default);
        Task<bool> IsReadAsync(Guid announcementId, Guid userId, CancellationToken ct = default);
    }
}
