

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;

namespace Horizon.Domain.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<Review?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct = default);
        Task<IEnumerable<Review>> GetByCourseAsync(Guid courseId, ReviewStatus? status = null, CancellationToken ct = default);
        Task<IEnumerable<Review>> GetPendingAsync(CancellationToken ct = default);
        Task<double> GetAverageRatingAsync(Guid courseId, CancellationToken ct = default);
        Task<Dictionary<int, int>> GetRatingDistributionAsync(Guid courseId, CancellationToken ct = default);
        Task<bool> HasReviewedAsync(Guid studentId, Guid courseId, CancellationToken ct = default);
    }
}
