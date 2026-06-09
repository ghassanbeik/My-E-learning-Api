using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Certificates.DownloadCertificate;
using Horizon.Application.Features.Certificates.GetCertificateById;
using Horizon.Application.Features.Certificates.GetMyCertificates;
using Horizon.Application.Features.Certificates.VerifyCertificate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/certificates")]
    public class CertificatesController : BaseController
    {
        private readonly IMediator _mediator;
        public CertificatesController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get my certificates</summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<CertificateDto>>), 200)]
        public async Task<IActionResult> GetMy(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetMyCertificatesQuery(UserId), ct));

        /// <summary>Get certificate by ID</summary>
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CertificateDto>), 200)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetCertificateByIdQuery(id, UserId), ct));

        /// <summary>Verify a certificate by number</summary>
        [HttpGet("verify/{number}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> Verify(string number, CancellationToken ct)
            => FromResult(await _mediator.Send(new VerifyCertificateQuery(number), ct));

        /// <summary>Download certificate as PDF</summary>
        [HttpGet("{id:guid}/download")]
        [Authorize]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> Download(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DownloadCertificateCommand(id, UserId), ct);
            if (!result.IsSuccess) return FromResult(result);
            return File(result.Value!, "application/pdf", $"certificate-{id}.pdf");
        }
    }
}
