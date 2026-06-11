

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Certificates.GetCertificateById
{
    public class GetCertificateByIdHandler
        : IRequestHandler<GetCertificateByIdQuery, Result<CertificateDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetCertificateByIdHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CertificateDto>> Handle(
            GetCertificateByIdQuery request, CancellationToken ct)
        {
            var cert = await _uow.Certificates.GetByIdAsync(request.CertificateId, ct);
            if (cert == null) return Result<CertificateDto>.NotFound("Certificate not found.");
            if (cert.StudentId != request.UserId) return Result<CertificateDto>.Forbidden();

            var course = await _uow.Courses.GetByIdAsync(cert.CourseId, ct);
            var user = await _uow.Users.GetByIdAsync(cert.StudentId, ct);

            return Result<CertificateDto>.Success(new CertificateDto(
                cert.Id, cert.CourseId, course?.Title ?? string.Empty,
                user?.FullName ?? string.Empty, cert.CertificateNumber,
                cert.IssueDate, cert.ExpiryDate, cert.VerificationUrl, cert.IsRevoked));
        }
    }
}
