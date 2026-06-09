

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Certificates.GetMyCertificates
{
    public record GetMyCertificatesQuery(Guid UserId) : IRequest<Result<List<CertificateDto>>>;
}
