

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CertificateServices;
using MediatR;

namespace Horizon.Application.Features.Certificates.DownloadCertificate
{
    public class DownloadCertificateHandler : IRequestHandler<DownloadCertificateCommand, Result<byte[]>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICertificateService _certs;

        public DownloadCertificateHandler(IUnitOfWork uow, ICertificateService certs) { _uow = uow; _certs = certs; }

        public async Task<Result<byte[]>> Handle(DownloadCertificateCommand request, CancellationToken ct)
        {
            var cert = await _uow.Certificates.GetByIdAsync(request.CertificateId, ct);
            if (cert == null) return Result<byte[]>.NotFound("Certificate not found.");
            if (cert.StudentId != request.UserId) return Result<byte[]>.Forbidden();
            if (cert.IsRevoked) return Result<byte[]>.Failure("Certificate has been revoked.");

            var pdf = await _certs.GeneratePdfAsync(request.CertificateId, ct);
            return Result<byte[]>.Success(pdf);
        }
    }

}
