

using Horizon.Application.Common;
using Horizon.Domain.Interfaces.Services.CertificateServices;
using MediatR;

namespace Horizon.Application.Features.Certificates.VerifyCertificate
{
    public class VerifyCertificateHandler : IRequestHandler<VerifyCertificateQuery, Result<bool>>
    {
        private readonly ICertificateService _certs;
        public VerifyCertificateHandler(ICertificateService certs) => _certs = certs;

        public async Task<Result<bool>> Handle(VerifyCertificateQuery request, CancellationToken ct)
        {
            var isValid = await _certs.VerifyAsync(request.CertificateNumber, ct);
            return Result<bool>.Success(isValid);
        }
    }
}
