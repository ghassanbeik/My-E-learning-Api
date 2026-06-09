
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId, ct);

        public async Task<Enrollment?> GetWithProgressAsync(Guid enrollmentId, CancellationToken ct = default)
            => await _dbSet
                .Include(e => e.Progresses).ThenInclude(p => p.Lesson)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId, ct);

        public async Task<IEnumerable<Enrollment>> GetByStudentAsync(Guid studentId, CancellationToken ct = default)
            => await _dbSet
                .Include(e => e.Course).ThenInclude(c => c.Instructor)
                .Where(e => e.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Enrollment>> GetByCourseAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(e => e.StudentId == studentId &&
                                          e.CourseId == courseId &&
                                          e.Status == EnrollmentStatus.Active, ct);

        public async Task UpdateProgressAsync(Guid enrollmentId, decimal percentage, CancellationToken ct = default)
        {
            var enrollment = await _dbSet.FindAsync(new object[] { enrollmentId }, ct);
            if (enrollment == null) return;
            enrollment.ProgressPercentage = percentage;
            enrollment.LastAccessedAt = DateTime.UtcNow;
            if (percentage >= 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedAt = DateTime.UtcNow;
            }
        }

        public async Task<int> GetEnrollmentCountAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet.CountAsync(e => e.CourseId == courseId, ct);

        public async Task<IEnumerable<Enrollment>> GetExpiringAsync(int daysAhead, CancellationToken ct = default)
            => await _dbSet
                .Where(e => e.ExpiresAt.HasValue &&
                            e.ExpiresAt <= DateTime.UtcNow.AddDays(daysAhead) &&
                            e.Status == EnrollmentStatus.Active)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
