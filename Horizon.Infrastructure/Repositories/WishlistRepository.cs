

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Wishlist>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Include(w => w.Course).ThenInclude(c => c.Instructor)
                .Where(w => w.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(w => w.UserId == userId && w.CourseId == courseId, ct);

        public async Task<bool> RemoveAsync(Guid userId, Guid courseId, CancellationToken ct = default)
        {
            var item = await _dbSet.FirstOrDefaultAsync(w => w.UserId == userId && w.CourseId == courseId, ct);
            if (item == null) return false;
            _dbSet.Remove(item);
            return true;
        }
    }

}
