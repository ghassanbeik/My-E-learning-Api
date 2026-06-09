

using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CertificateServices;
using Horizon.Domain.Interfaces.Services.StorageServices;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _storage;
        private readonly ILogger<CertificateService> _logger;

        public CertificateService(IUnitOfWork uow, IFileStorageService storage, ILogger<CertificateService> logger)
        {
            _uow = uow;
            _storage = storage;
            _logger = logger;
        }

        public async Task<Certificate> GenerateAsync(Guid enrollmentId, CancellationToken ct = default)
        {
            var existing = await _uow.Certificates.GetByEnrollmentAsync(enrollmentId, ct);
            if (existing != null) return existing;

            var enrollment = await _uow.Enrollments.GetWithProgressAsync(enrollmentId, ct);
            if (enrollment == null) throw new InvalidOperationException("Enrollment not found.");

            var certNumber = $"CERT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            var certificate = new Certificate
            {
                EnrollmentId = enrollmentId,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId,
                CertificateNumber = certNumber,
                IssueDate = DateTime.UtcNow,
                VerificationUrl = $"https://horizon.com/verify/{certNumber}",
            };

            await _uow.Certificates.AddAsync(certificate, ct);
            await _uow.SaveChangesAsync(ct);
            return certificate;
        }

        public Task<byte[]> GeneratePdfAsync(Guid certificateId, CancellationToken ct = default)
        {
            // PDF generation — integrate with a library like QuestPDF or iText in production
            _logger.LogInformation("PDF generation requested for certificate {CertificateId}", certificateId);
            return Task.FromResult(Array.Empty<byte>());
        }

        public async Task<bool> VerifyAsync(string certificateNumber, CancellationToken ct = default)
            => await _uow.Certificates.VerifyAsync(certificateNumber, ct);

        public async Task RevokeAsync(Guid certificateId, string reason, CancellationToken ct = default)
        {
            var certificate = await _uow.Certificates.GetByIdAsync(certificateId, ct);
            if (certificate == null) return;
            certificate.IsRevoked = true;
            certificate.RevokeReason = reason;
            await _uow.Certificates.UpdateAsync(certificate);
            await _uow.SaveChangesAsync(ct);
        }
    }

}
