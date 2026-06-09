
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Enrollments.GetEnrollmentDetail
{
    public record GetEnrollmentDetailQuery(Guid EnrollmentId, Guid StudentId) : IRequest<Result<EnrollmentDetailDto>>;

}
