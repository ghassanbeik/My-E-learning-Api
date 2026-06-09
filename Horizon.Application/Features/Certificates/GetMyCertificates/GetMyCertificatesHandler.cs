

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Certificates.GetMyCertificates
{
    public class GetMyCertificatesHandler : IRequestHandler<GetMyCertificatesQuery, Result<List<CertificateDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetMyCertificatesHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<CertificateDto>>> Handle(GetMyCertificatesQuery request, CancellationToken ct)
        {
            var certs = await _uow.Certificates.GetByStudentAsync(request.UserId, ct);
            var results = new List<CertificateDto>();

            foreach (var c in certs)
            {
                var course = await _uow.Courses.GetByIdAsync(c.CourseId, ct);
                var user = await _uow.Users.GetByIdAsync(c.StudentId, ct);
                results.Add(new CertificateDto(c.Id, c.CourseId, course?.Title ?? string.Empty,
                    user?.FullName ?? string.Empty, c.CertificateNumber,
                    c.IssueDate, c.ExpiryDate, c.VerificationUrl, c.IsRevoked));
            }

            return Result<List<CertificateDto>>.Success(results);
        }
    }
}
