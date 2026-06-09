

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Announcement>> GetByCourseAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(a => a.Instructor)
                .Where(a => a.CourseId == courseId)
                .OrderByDescending(a => a.IsPinned)
                .ThenByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Announcement>> GetUnreadByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Include(a => a.Instructor)
                .Include(a => a.Course)
                .Where(a => !a.ReadBy.Any(r => r.UserId == userId))
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task MarkAsReadAsync(Guid announcementId, Guid userId, CancellationToken ct = default)
        {
            var exists = await _context.Set<AnnouncementRead>()
                .AnyAsync(ar => ar.AnnouncementId == announcementId && ar.UserId == userId, ct);
            if (!exists)
                await _context.Set<AnnouncementRead>().AddAsync(
                    new AnnouncementRead { AnnouncementId = announcementId, UserId = userId }, ct);
        }

        public async Task<bool> IsReadAsync(Guid announcementId, Guid userId, CancellationToken ct = default)
            => await _context.Set<AnnouncementRead>()
                .AnyAsync(ar => ar.AnnouncementId == announcementId && ar.UserId == userId, ct);
    }
}
