

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Certificates.VerifyCertificate
{
    public record VerifyCertificateQuery(string CertificateNumber) : IRequest<Result<bool>>;

}
