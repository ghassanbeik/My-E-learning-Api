

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ICourseCategoryRepository : IRepository<CourseCategory>
    {
        Task<bool> ExistsAsync(Guid categoryId, CancellationToken ct = default);
    }
}
