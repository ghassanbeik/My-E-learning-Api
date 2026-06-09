using Horizon.Domain.Entities;

namespace Horizon.Domain.Interfaces.Services.CertificateServices
{
    public interface ICertificateService
    {
        Task<Certificate> GenerateAsync(Guid enrollmentId, CancellationToken ct = default);
        Task<byte[]> GeneratePdfAsync(Guid certificateId, CancellationToken ct = default);
        Task<bool> VerifyAsync(string certificateNumber, CancellationToken ct = default);
        Task RevokeAsync(Guid certificateId, string reason, CancellationToken ct = default);
    }
}
