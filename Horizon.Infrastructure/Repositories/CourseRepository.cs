

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Domain.Shared;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Course?> GetWithDetailsAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Sections.OrderBy(s => s.DisplayOrder))
                    .ThenInclude(s => s.Lessons.OrderBy(l => l.DisplayOrder))
                .Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.CourseTags).ThenInclude(ct2 => ct2.Tag)
                .Include(c => c.Reviews.Where(r => r.Status == ReviewStatus.Approved).Take(5))
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId, ct);

        public async Task<Course?> GetWithSectionsAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.Sections.OrderBy(s => s.DisplayOrder))
                    .ThenInclude(s => s.Lessons.OrderBy(l => l.DisplayOrder))
                .FirstOrDefaultAsync(c => c.Id == courseId, ct);

        public async Task<Course?> GetWithReviewsAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.Reviews.Where(r => r.Status == ReviewStatus.Approved))
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(c => c.Id == courseId, ct);

        public async Task<IEnumerable<Course>> GetByInstructorAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Course>> GetFeaturedCoursesAsync(int count, CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.IsFeatured && c.Status == CourseStatus.Published)
                .Include(c => c.Instructor)
                .Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .OrderByDescending(c => c.TotalStudents)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Course>> GetTopRatedCoursesAsync(int count, CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.Status == CourseStatus.Published && c.TotalReviews >= 10)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Course>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.Status == CourseStatus.Published &&
                            c.CourseCategories.Any(cc => cc.CategoryId == categoryId))
                .Include(c => c.Instructor)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<PagedResult<Course>> SearchCoursesAsync(CourseSearchParams p, CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(p.Query))
                query = query.Where(c => c.Title.Contains(p.Query) ||
                                         c.Description.Contains(p.Query) ||
                                         c.Instructor.FirstName.Contains(p.Query) ||
                                         c.Instructor.LastName.Contains(p.Query));

            if (p.CategoryId.HasValue)
                query = query.Where(c => c.CourseCategories.Any(cc => cc.CategoryId == p.CategoryId));

            if (p.TagId.HasValue)
                query = query.Where(c => c.CourseTags.Any(ct2 => ct2.TagId == p.TagId));

            if (p.Level.HasValue)
                query = query.Where(c => c.Level == p.Level);

            if (p.Status.HasValue)
                query = query.Where(c => c.Status == p.Status);
            else
                query = query.Where(c => c.Status == CourseStatus.Published);

            if (p.MinPrice.HasValue)
                query = query.Where(c => c.Price >= p.MinPrice);

            if (p.MaxPrice.HasValue)
                query = query.Where(c => c.Price <= p.MaxPrice);

            if (p.MinRating.HasValue)
                query = query.Where(c => c.AverageRating >= p.MinRating);

            if (!string.IsNullOrWhiteSpace(p.Language))
                query = query.Where(c => c.Language == p.Language);

            if (p.IsFeatured.HasValue)
                query = query.Where(c => c.IsFeatured == p.IsFeatured);

            if (p.IsFree.HasValue)
                query = p.IsFree.Value
                    ? query.Where(c => c.Price == 0)
                    : query.Where(c => c.Price > 0);

            var totalCount = await query.CountAsync(ct);

            query = p.SortBy switch
            {
                "Price" => p.Descending ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price),
                "Rating" => p.Descending ? query.OrderByDescending(c => c.AverageRating) : query.OrderBy(c => c.AverageRating),
                "Students" => p.Descending ? query.OrderByDescending(c => c.TotalStudents) : query.OrderBy(c => c.TotalStudents),
                "Title" => p.Descending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
                _ => p.Descending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            };

            var items = await query
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .ToListAsync(ct);

            return new PagedResult<Course>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = p.Page,
                PageSize = p.PageSize,
            };
        }

        public async Task UpdateRatingAsync(Guid courseId, double rating, int totalReviews, CancellationToken ct = default)
        {
            var course = await _dbSet.FindAsync(new object[] { courseId }, ct);
            if (course == null) return;
            course.AverageRating = rating;
            course.TotalReviews = totalReviews;
        }

        public async Task IncrementStudentCountAsync(Guid courseId, CancellationToken ct = default)
        {
            var course = await _dbSet.FindAsync(new object[] { courseId }, ct);
            if (course != null) course.TotalStudents++;
        }

        public async Task DecrementStudentCountAsync(Guid courseId, CancellationToken ct = default)
        {
            var course = await _dbSet.FindAsync(new object[] { courseId }, ct);
            if (course != null && course.TotalStudents > 0) course.TotalStudents--;
        }

        public async Task<bool> IsTitleUniqueAsync(string title, Guid? excludeId = null, CancellationToken ct = default)
            => !await _dbSet.AnyAsync(c => c.Title == title && (!excludeId.HasValue || c.Id != excludeId), ct);
    }
}
