

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct = default);
        Task ClearCartAsync(Guid userId, CancellationToken ct = default);
        Task<decimal> GetCartTotalAsync(Guid userId, CancellationToken ct = default);
    }
}
