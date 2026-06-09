

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IInstructorProfileRepository : IRepository<InstructorProfile>
    {
        Task<InstructorProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<IEnumerable<InstructorProfile>> GetTopInstructorsAsync(int count, CancellationToken ct = default);
        Task UpdateEarningsAsync(Guid userId, decimal amount, CancellationToken ct = default);
    }
}
