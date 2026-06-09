

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Review?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId, ct);

        public async Task<IEnumerable<Review>> GetByCourseAsync(Guid courseId, ReviewStatus? status = null, CancellationToken ct = default)
        {
            var query = _dbSet.Include(r => r.Student).Where(r => r.CourseId == courseId);
            if (status.HasValue) query = query.Where(r => r.Status == status);
            return await query.OrderByDescending(r => r.CreatedAt).AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Review>> GetPendingAsync(CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Course)
                .Where(r => r.Status == ReviewStatus.Pending)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<double> GetAverageRatingAsync(Guid courseId, CancellationToken ct = default)
        {
            var reviews = await _dbSet
                .Where(r => r.CourseId == courseId && r.Status == ReviewStatus.Approved)
                .ToListAsync(ct);
            return reviews.Count == 0 ? 0 : reviews.Average(r => r.Rating);
        }

        public async Task<Dictionary<int, int>> GetRatingDistributionAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Where(r => r.CourseId == courseId && r.Status == ReviewStatus.Approved)
                .GroupBy(r => r.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count, ct);

        public async Task<bool> HasReviewedAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId, ct);
    }

}
