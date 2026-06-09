

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Certificates.DownloadCertificate
{
    public record DownloadCertificateCommand(Guid CertificateId, Guid UserId) : IRequest<Result<byte[]>>;

}
