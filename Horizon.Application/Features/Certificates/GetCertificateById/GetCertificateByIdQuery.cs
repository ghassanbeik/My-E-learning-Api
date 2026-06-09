

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Certificates.GetCertificateById
{
    public record GetCertificateByIdQuery(Guid CertificateId, Guid UserId) : IRequest<Result<CertificateDto>>;

}
