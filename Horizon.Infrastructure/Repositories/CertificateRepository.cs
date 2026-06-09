

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Certificate?> GetByNumberAsync(string certificateNumber, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.CertificateNumber == certificateNumber, ct);

        public async Task<Certificate?> GetByEnrollmentAsync(Guid enrollmentId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.EnrollmentId == enrollmentId, ct);

        public async Task<IEnumerable<Certificate>> GetByStudentAsync(Guid studentId, CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.Course)
                .Where(c => c.StudentId == studentId && !c.IsRevoked)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<bool> VerifyAsync(string certificateNumber, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.CertificateNumber == certificateNumber && !c.IsRevoked, ct);
    }
}
