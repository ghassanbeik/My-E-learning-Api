using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Horizon.Infrastructure.Repositories
{
    public class InstructorSubscriberRepository : Repository<InstructorSubscriber>, IInstructorSubscriberRepository
    {
        public InstructorSubscriberRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> IsSubscribedAsync(Guid instructorId, Guid subscriberId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(s => s.InstructorId == instructorId && s.SubscriberId == subscriberId, ct);

        public async Task<int> GetSubscriberCountAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet.CountAsync(s => s.InstructorId == instructorId, ct);

        public async Task<IEnumerable<UserInfo>> GetSubscribersAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet
                .Where(s => s.InstructorId == instructorId)
                .Select(s => s.Subscriber)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
