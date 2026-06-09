

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CartItem>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Include(ci => ci.Course).ThenInclude(c => c.Instructor)
                .Where(ci => ci.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(ci => ci.UserId == userId && ci.CourseId == courseId, ct);

        public async Task ClearCartAsync(Guid userId, CancellationToken ct = default)
        {
            var items = await _dbSet.Where(ci => ci.UserId == userId).ToListAsync(ct);
            _dbSet.RemoveRange(items);
        }

        public async Task<decimal> GetCartTotalAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Include(ci => ci.Course)
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Course.Price, ct);
    }
}
