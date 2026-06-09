
using Horizon.Domain.Entities;
using Horizon.Domain.Shared;

namespace Horizon.Domain.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course?> GetWithDetailsAsync(Guid courseId, CancellationToken ct = default);
        Task<Course?> GetWithSectionsAsync(Guid courseId, CancellationToken ct = default);
        Task<Course?> GetWithReviewsAsync(Guid courseId, CancellationToken ct = default);
        Task<IEnumerable<Course>> GetByInstructorAsync(Guid instructorId, CancellationToken ct = default);
        Task<IEnumerable<Course>> GetFeaturedCoursesAsync(int count, CancellationToken ct = default);
        Task<IEnumerable<Course>> GetTopRatedCoursesAsync(int count, CancellationToken ct = default);
        Task<IEnumerable<Course>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default);
        Task<PagedResult<Course>> SearchCoursesAsync(CourseSearchParams searchParams, CancellationToken ct = default);
        Task UpdateRatingAsync(Guid courseId, double rating, int totalReviews, CancellationToken ct = default);
        Task IncrementStudentCountAsync(Guid courseId, CancellationToken ct = default);
        Task DecrementStudentCountAsync(Guid courseId, CancellationToken ct = default);
        Task<bool> IsTitleUniqueAsync(string title, Guid? excludeId = null, CancellationToken ct = default);
    }
}
