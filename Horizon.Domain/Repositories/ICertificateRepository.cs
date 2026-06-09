

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<Certificate?> GetByNumberAsync(string certificateNumber, CancellationToken ct = default);
        Task<Certificate?> GetByEnrollmentAsync(Guid enrollmentId, CancellationToken ct = default);
        Task<IEnumerable<Certificate>> GetByStudentAsync(Guid studentId, CancellationToken ct = default);
        Task<bool> VerifyAsync(string certificateNumber, CancellationToken ct = default);
    }
}
