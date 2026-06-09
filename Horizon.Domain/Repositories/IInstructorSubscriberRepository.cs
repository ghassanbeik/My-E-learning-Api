

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IInstructorSubscriberRepository : IRepository<InstructorSubscriber>
    {
        Task<bool> IsSubscribedAsync(Guid instructorId, Guid subscriberId, CancellationToken ct = default);
        Task<int> GetSubscriberCountAsync(Guid instructorId, CancellationToken ct = default);
        Task<IEnumerable<UserInfo>> GetSubscribersAsync(Guid instructorId, CancellationToken ct = default);
    }
}
