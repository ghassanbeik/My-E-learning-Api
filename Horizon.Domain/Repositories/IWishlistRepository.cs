

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct = default);
        Task<bool> RemoveAsync(Guid userId, Guid courseId, CancellationToken ct = default);
    }
}
