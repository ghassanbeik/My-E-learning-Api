using Horizon.Domain.Entities;


namespace Horizon.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken ct = default);
        Task<IEnumerable<Category>> GetWithSubCategoriesAsync(CancellationToken ct = default);
        Task<Category?> GetWithCoursesAsync(Guid categoryId, CancellationToken ct = default);
        Task<IEnumerable<Category>> GetFeaturedAsync(CancellationToken ct = default);
        Task<bool> NameExistsAsync(string name, Guid? excludeId = null, CancellationToken ct = default);
    }
}
