
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task<IEnumerable<Section>> GetByCourseAsync(Guid courseId, CancellationToken ct = default);
        Task<Section?> GetWithLessonsAsync(Guid sectionId, CancellationToken ct = default);
        Task ReorderAsync(Guid courseId, IEnumerable<(Guid SectionId, int Order)> orders, CancellationToken ct = default);
    }
}
